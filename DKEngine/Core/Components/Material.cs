﻿/*
* (C) 2017 David Knieradl
*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DKEngine.Core.Components
{
    /// <summary>
    /// Low-Memory Material
    /// </summary>
    public sealed class Material
    {
        /// <summary>
        /// Source image used as Texture
        /// </summary>
        public readonly Bitmap Texture = null;

        /// <summary>
        /// Represents scaled length of image in pixels
        /// </summary>
        public readonly int Width = 0;

        /// <summary>
        /// Represents scaled height of image in pixels
        /// </summary>
        public readonly int Height = 0;

        /// <summary>
        /// Number of frames
        /// </summary>
        public readonly int Frames = 1;

        /// <summary>
        /// Total duration of animated image
        /// </summary>
        public readonly int Duration = 1;

        /// <summary>
        /// Duration between two frames of animation
        /// </summary>
        public readonly int DurationPerFrame = 1;

        /// <summary>
        /// Returns true if image is animated
        /// </summary>
        public readonly bool IsAnimated = false;

        /// <summary>
        /// Returns true if image is looped
        /// </summary>
        public readonly bool IsLooped = false;

        private int _SelectedLayer = -1;
        private FrameDimension _FrameDim = null;
        private BitmapData _BitmapData = null;
        private byte[] _Data = null;
        private byte _BytesPerPixel = 0;

        /// <summary>
        /// Loads image and creates new material
        /// </summary>
        /// <param name="source">Source image</param>
        public Material(Image source)
        {
            if (source != null)
            {
                _FrameDim = new FrameDimension(source.FrameDimensionsList[0]);
                Frames = source.GetFrameCount(_FrameDim);

                Texture = (Bitmap)source;

                Width = source.Width;
                Height = source.Height;

                if (ImageAnimator.CanAnimate(source))
                {
                    int delay = 0;
                    int this_delay = 0;
                    int index = 0;

                    for (int i = 0; i < Frames; i++)
                    {
                        this_delay = BitConverter.ToInt32(source.GetPropertyItem(20736).Value, index) * 10;
                        delay += (this_delay < 1 ? 33 : this_delay);
                        index += 4;
                    }

                    Duration = delay;
                    DurationPerFrame = Duration / Frames;
                    IsAnimated = true;
                    IsLooped = BitConverter.ToInt16(source.GetPropertyItem(20737).Value, 0) != 1;
                }

                switch (source.PixelFormat)
                {
                    case PixelFormat.Format32bppArgb:
                        _BytesPerPixel = 4;
                        break;

                    case PixelFormat.Format24bppRgb:
                        _BytesPerPixel = 3;
                        break;

                    default:
                        throw new Exception("Unsupported");
                }

                _Data = new byte[Width * Height * _BytesPerPixel];
            }
        }

        /// <summary>
        /// Creates new material with given color and scales it by parent's given scales
        /// </summary>
        /// <param name="clr">Source color</param>
        /// <param name="Size">Vector3 used for material size</param>
        public Material(Color clr, Vector3 Size)
        {
            this.Width = (int)(Size.X < 1 ? 1 : Size.X);
            this.Height = (int)(Size.Y < 1 ? 1 : Size.Y);

            Frames = 1;

            _BytesPerPixel = 4;

            int size = Width * Height * _BytesPerPixel;
            _Data = new byte[size];

            for (int index = 0; index < size; index += _BytesPerPixel)
            {
                _Data[index + 3] = clr.A;
                _Data[index + 2] = clr.R;
                _Data[index + 1] = clr.G;
                _Data[index] = clr.B;
            }

            unsafe
            {
                fixed (byte* data = _Data)
                {
                    using (Bitmap tmp = new Bitmap(Width,
                                             Height,
                                             Width * _BytesPerPixel,
                                             PixelFormat.Format32bppArgb,
                                             new IntPtr(data)))
                    {
                        Texture = new Bitmap(tmp);
                    }
                }
            }

            _FrameDim = new FrameDimension(Texture.FrameDimensionsList[0]);
        }

        /// <summary>
        /// Creates new material with given color and scales it by parent's given scales
        /// </summary>
        /// <param name="clr">Source color</param>
        /// <param name="Parent">GameObject used for material size</param>
        public Material(Color clr, GameObject Parent)
            : this(clr, Parent.Transform.Dimensions)
        { }

        /// <summary>
        /// Render material into engine image buffer
        /// </summary>
        /// <param name="Parent">I3Dimensional for coordiantions</param>
        public void Render(GameObject Parent, Color? ReColor = null)
        {
            int AnimationState = Parent.Animator != null ? Parent.Animator.AnimationState : 0;
            bool HasShadow = Parent.HasShadow;

            if (_SelectedLayer != AnimationState)
            {
                if (_BitmapData != null)
                    Texture.UnlockBits(_BitmapData);
                Texture.SelectActiveFrame(_FrameDim, AnimationState);
                _BitmapData = Texture.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, Texture.PixelFormat);
                Marshal.Copy(_BitmapData.Scan0, _Data, 0, _Data.Length);
                _SelectedLayer = AnimationState;
            }

            float CamX = Engine.BaseCam != null ? Engine.BaseCam.X : 0;
            float CamY = Engine.BaseCam != null ? Engine.BaseCam.Y : 0;

            int x = (int)(Parent.Transform.Position.X - CamX);
            int y = (int)(Parent.Transform.Position.Y - CamY);

            float RasteredHeight = this.Height * Parent.Transform.Scale.Y;
            float RasteredWidth = this.Width * Parent.Transform.Scale.X;

            float NonRasteredWidthRatio = 1 / Parent.Transform.Scale.X;
            float NonRasteredHeightRatio = 1 / Parent.Transform.Scale.Y;

            float NonRasteredHeight = 0;
            float NonRasteredWidth = 0;
            if (ReColor == null)
            {
                for (int row = 0; row < RasteredHeight; row++)
                {
                    NonRasteredWidth = 0;

                    if (y + row >= Engine.Render.RenderHeight)
                        break;

                    for (int column = 0; column < RasteredWidth; column++)
                    {
                        if (x + column >= Engine.Render.RenderWidth)
                            break;

                        if (IsOnScreen(x + column, y + row))
                        {
                            int offset = (int)(3 * ((y + row) * Engine.Render.RenderWidth + (x + column)));
                            int keyOffset = (int)((y + row) * Engine.Render.RenderWidth + (x + column));

                            int tempColumn = (int)NonRasteredWidth;
                            int tempRow = (int)NonRasteredHeight;

                            int index = _BytesPerPixel * (tempRow * Width + tempColumn);

                            if (Engine.Render.imageBufferKey[keyOffset] != 255 && _BytesPerPixel == 4 ? _Data[index + 3] != 0 : true)
                            {
                                Color temp = MixPixel(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset],
                                                        _BytesPerPixel == 4 ? _Data[index + 3] : (byte)255, _Data[index + 2], _Data[index + 1], _Data[index]);

                                Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                Engine.Render.imageBuffer[offset] = temp.B;
                                Engine.Render.imageBuffer[offset + 1] = temp.G;
                                Engine.Render.imageBuffer[offset + 2] = temp.R;
                            }
                        }

                        NonRasteredWidth += NonRasteredWidthRatio;
                    }

                    NonRasteredHeight += NonRasteredHeightRatio;
                }
            }
            else
            {
                Color tempColor = (Color)ReColor;

                for (int row = 0; row < RasteredHeight; row++)
                {
                    NonRasteredWidth = 0;

                    if (y + row >= Engine.Render.RenderHeight)
                        break;

                    for (int column = 0; column < RasteredWidth; column++)
                    {
                        if (x + column >= Engine.Render.RenderWidth)
                            break;

                        if (IsOnScreen(x + column, y + row))
                        {
                            int offset = (int)(3 * ((y + row) * Engine.Render.RenderWidth + (x + column)));
                            int keyOffset = (int)((y + row) * Engine.Render.RenderWidth + (x + column));

                            int tempColumn = (int)NonRasteredWidth;
                            int tempRow = (int)NonRasteredHeight;

                            int index = _BytesPerPixel * (tempRow * Width + tempColumn);

                            if (Engine.Render.imageBufferKey[keyOffset] != 255 && _BytesPerPixel == 4 ? _Data[index + 3] != 0 : true)
                            {
                                Color temp = MixPixel(Color.FromArgb(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset]),
                                                        tempColor);

                                Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                Engine.Render.imageBuffer[offset] = temp.B;
                                Engine.Render.imageBuffer[offset + 1] = temp.G;
                                Engine.Render.imageBuffer[offset + 2] = temp.R;
                            }
                        }

                        NonRasteredWidth += NonRasteredWidthRatio;
                    }

                    NonRasteredHeight += NonRasteredHeightRatio;
                }
            }

            if (HasShadow)
            {
                NonRasteredHeight = 0;
                NonRasteredWidth = 0;

                x++;
                y++;

                for (int row = 0; row < RasteredHeight; row++)
                {
                    NonRasteredWidth = 0;

                    if (y + row >= Engine.Render.RenderHeight)
                        break;

                    for (int column = 0; column < RasteredWidth; column++)
                    {
                        if (x + column >= Engine.Render.RenderWidth)
                            break;

                        if (IsOnScreen(x + column, y + row))
                        {
                            int offset = (int)(3 * ((y + row) * Engine.Render.RenderWidth + (x + column)));
                            int keyOffset = (int)((y + row) * Engine.Render.RenderWidth + (x + column));

                            int tempColumn = (int)NonRasteredWidth;
                            int tempRow = (int)NonRasteredHeight;

                            int index = _BytesPerPixel * (tempRow * Width + tempColumn);

                            if (Engine.Render.imageBufferKey[keyOffset] != 255 && _BytesPerPixel == 4 ? _Data[index + 3] != 0 : true)
                            {
                                Color temp = MixPixel(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset],
                                                      (byte)192, (byte)0, (byte)0, (byte)0);

                                Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                Engine.Render.imageBuffer[offset] = temp.B;
                                Engine.Render.imageBuffer[offset + 1] = temp.G;
                                Engine.Render.imageBuffer[offset + 2] = temp.R;
                            }
                        }

                        NonRasteredWidth += NonRasteredWidthRatio;
                    }

                    NonRasteredHeight += NonRasteredHeightRatio;
                }
            }
        }

        public Color MixPixel(byte topA, byte topR, byte topG, byte topB, byte bottomA, byte bottomR, byte bottomG, byte bottomB)
        {
            if (topA == 0)
                return Color.FromArgb(bottomA, bottomR, bottomG, bottomB);

            if (bottomA == 0)
                return Color.FromArgb(topA, topR, topG, topB);

            float opacityTop = (float)topA / 255;

            byte newA = (byte)(topA + bottomA >= 255 ? 255 : topA + bottomA);
            byte A = (byte)(newA - topA);

            float opacityBottom = (float)A / 255;

            byte R = (byte)(topR * opacityTop + bottomR * opacityBottom);
            byte G = (byte)(topG * opacityTop + bottomG * opacityBottom);
            byte B = (byte)(topB * opacityTop + bottomB * opacityBottom);

            return Color.FromArgb(newA, R, G, B);
        }

        public Color MixPixel(Color top, Color bottom)
        {
            if (top.A == 0)
                return bottom;

            if (bottom.A == 0)
                return top;

            float opacityTop = (float)top.A / 255;

            byte newA = (byte)(top.A + bottom.A >= 255 ? 255 : top.A + bottom.A);
            byte A = (byte)(newA - top.A);

            float opacityBottom = (float)A / 255;

            byte R = (byte)(top.R * opacityTop + bottom.R * opacityBottom);
            byte G = (byte)(top.G * opacityTop + bottom.G * opacityBottom);
            byte B = (byte)(top.B * opacityTop + bottom.B * opacityBottom);

            return Color.FromArgb(newA, R, G, B);
        }

        private bool IsOnScreen(float x, float y)
        {
            return x >= 0 && x < Engine.Render.RenderWidth && y >= 0 && y < Engine.Render.RenderHeight;
        }
    }
}