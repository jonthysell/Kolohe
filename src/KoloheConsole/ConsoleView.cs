// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Kolohe.CLI
{
    class ConsoleView : SingleScreenView
    {
        public char[,] Buffer { get; private set; }

        public ConsoleView() : base(Console.WindowWidth, Console.WindowHeight)
        {
            Console.CursorVisible = false;
        }

        public override void Resize(int width, int height)
        {
            base.Resize(width, height);
            Buffer = new char[width, height];
        }

        public override async Task<EngineInput> ReadInputAsync()
        {
            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.NumPad8:
                case ConsoleKey.UpArrow:
                    return EngineInput.MoveUp;
                case ConsoleKey.NumPad9:
                    return EngineInput.MoveUpRight;
                case ConsoleKey.NumPad6:
                case ConsoleKey.RightArrow:
                    return EngineInput.MoveRight;
                case ConsoleKey.NumPad3:
                    return EngineInput.MoveDownRight;
                case ConsoleKey.NumPad2:
                case ConsoleKey.DownArrow:
                    return EngineInput.MoveDown;
                case ConsoleKey.NumPad1:
                    return EngineInput.MoveDownLeft;
                case ConsoleKey.NumPad4:
                case ConsoleKey.LeftArrow:
                    return EngineInput.MoveLeft;
                case ConsoleKey.NumPad7:
                    return EngineInput.MoveUpLeft;
                case ConsoleKey.NumPad5:
                    return EngineInput.Wait;
                case ConsoleKey.R:
                    return input.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.RefreshView : EngineInput.None;
            }

            return EngineInput.None;
        }

        protected override bool SyncScreenDimensions()
        {
            bool refresh = false;
            if (Console.WindowWidth != ScreenWidth || Console.WindowHeight != ScreenHeight)
            {
                Resize(Console.WindowWidth, Console.WindowHeight);
                refresh = true;
            }

            if (Console.WindowWidth != Console.BufferWidth || Console.WindowHeight != Console.BufferHeight)
            {
                Console.SetWindowPosition(0, 0);
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
                refresh = true;
            }

            return refresh;
        }

        protected override async Task DrawMapTileAsync(int x, int y, MapTile tile, bool forceRefresh)
        {
            char oldChar = Buffer[x, y];
            char newChar;

            switch (tile)
            {
                case MapTile.Floor:
                    newChar = '.';
                    break;
                case MapTile.Wall:
                    newChar = '#';
                    break;
                default:
                    newChar = ' ';
                    break;
            }
            
            Buffer[x, y] = newChar;

            if (oldChar != newChar || forceRefresh)
            {
                Console.SetCursorPosition(x, y);
                Console.CursorVisible = false;
                Console.Write(newChar);
            }
        }

        protected override async Task DrawEntityAsync(int x, int y, bool forceRefresh)
        {
            char oldChar = Buffer[x, y];
            char newChar = '@';

            Buffer[x, y] = newChar;

            if (oldChar != newChar || forceRefresh)
            {
                Console.SetCursorPosition(x, y);
                Console.CursorVisible = false;
                Console.Write(newChar);
            }
        }
    }
}
