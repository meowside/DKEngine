﻿using DKBasicEngine_1_0;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine
{
    class Test : Scene
    {
        
        public Test()
            :base()
        {
            /*Database.AddNewGameObjectMaterial("animatedDemo", new Material(Image.FromFile(@"giphy-downsized-large.gif")));

            GameObject t = new GameObject(this, null)
            {
                TypeName = "animatedDemo",
                Z = -1
            };

            t.Animator.Settings = AnimationLoop.Endless;

            for(int i = 0; i < 10; i++)
            {
                string tx = i.ToString();

                new Button(this)
                {
                    Text = tx + " Button",
                    OnClick = () => Debug.WriteLine(tx + " Button clicked"),
                    Y = i * 15,
                    width = 100,
                    height = 8,
                    FontSize = 2,
                    Foreground = Color.FromArgb(i * 0x0F, i * 0x0F, 0x00, i * 0x0F),
                    Background = Color.FromArgb(i * 0x0F, i * 0x0F, i * 0x0F, i * 0x0F)
                };
            }*/

            /*new Button(this)
            {
                Text = "GG",
                OnClick = () => Debug.WriteLine("ButtonJede"),
                X = 50,
                width = 100,
                height = 50,
                FontSize = 4,
                Foreground = Color.FromArgb(0xCD, 0xFF, 0xFF, 0xFF),
                Background = Color.FromArgb(0x80, 0xFF, 0x00, 0XFF)
            };

            for(int a = 01; a < 2; a++)
                for(int i = 0; i < 16; i++)
                    for (int j = 0; j < 16; j++)
                        this.Model.Add(new GameObject(this, null)
                        {
                            X = i * 16,
                            ScaleX = 2,
                            Z = -a,
                            Y = j * 16,
                            TypeName = "border"
                        });

            for (int a = 01; a < 2; a++)
                for (int i = 1; i < 32; i++)
                    for (int j = 1; j < 64; j++)
                        this.Model.Add(new GameObject(this, null)
                        {
                            X = i * 1024,
                            ScaleX = 2,
                            Z = -a,
                            Y = j * 16,
                            TypeName = "border"
                        });*/
        }
    }
}
