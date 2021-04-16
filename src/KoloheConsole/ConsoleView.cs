// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Kolohe.CLI
{
    class ConsoleView : SingleScreenView<ConsoleTile>
    {
        public readonly ConsoleTile DefaultTile;
        
        public ConsoleView() : base(Console.WindowWidth, Console.WindowHeight)
        {
            DefaultTile = new ConsoleTile()
            {
                Char = ' ',
                BackgroundColor = Console.BackgroundColor,
                ForegroundColor = Console.ForegroundColor,
            };
        }

        public override async Task<EngineInput> ReadInputAsync()
        {
            if (!Console.KeyAvailable)
            {
                return EngineInput.None;
            }

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
            if (Console.WindowWidth != ScreenBounds.Width || Console.WindowHeight != ScreenBounds.Height)
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

        protected override ConsoleTile GetMapTile(MapTile mapTile, bool player)
        {
            var consoleTile = new ConsoleTile();

            switch (mapTile)
            {
                case MapTile.Ocean:
                    consoleTile.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case MapTile.Sand:
                    consoleTile.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                default:
                    return new ConsoleTile();
            }

            if (player)
            {
                consoleTile.Char = '@';
                consoleTile.ForegroundColor = ConsoleColor.Green;
            }

            return consoleTile;
        }

        protected override async Task ClearScreenAsync()
        {
            Console.ResetColor();
            Console.Clear();
        }

        protected override async Task DrawScreenTileAsync(int x, int y)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(x, y);

            var tile = TileBuffer[x, y] ?? DefaultTile;

            Console.BackgroundColor = tile.BackgroundColor;
            Console.ForegroundColor = tile.ForegroundColor;
            Console.Write(tile.Char);
        }
    }
}
