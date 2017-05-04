﻿/*
* (C) 2017 David Knieradl
*/

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;

namespace DKEngine.Core.Ext
{
    public static class WindowControl
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            public short X;
            public short Y;

            public COORD(short x, short y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleDisplayMode(IntPtr ConsoleOutput, uint Flags, out COORD NewScreenBufferDimensions);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        private static IntPtr hConsole = GetStdHandle(-11);
        private static COORD xy = new COORD(100, 100);
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        
        internal static void WindowInit()
        {
            Console.CursorVisible = false;
            Console.SetOut(TextWriter.Null);
            Console.SetIn(TextReader.Null);

            Console.BufferHeight = Console.LargestWindowHeight;
            Console.BufferWidth = Console.LargestWindowWidth;

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Clear();

            WindowSizeChecker(null, null);

            Timer windowChecker = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = 1000f
            };
            windowChecker.Elapsed += WindowSizeChecker;

            windowChecker.Start();
        }

        private static void WindowSizeChecker(object sender, ElapsedEventArgs e)
        {
            if (Console.WindowHeight != Console.LargestWindowHeight || Console.WindowWidth != Console.LargestWindowWidth)
            {
                SetConsoleDisplayMode(hConsole, 1, out xy);
                Console.CursorVisible = false;
            }
        }
    }
}