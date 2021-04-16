﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Kolohe.CLI
{
    class ConsoleView : SingleScreenView<ConsoleTile>
    {
        public readonly ConsoleTile DefaultTile = new ConsoleTile();

        public ConsoleView() : base(Console.WindowWidth, Console.WindowHeight) { }

        public override async Task<EngineInput> ReadInputAsync()
        {
            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.NumPad8:
                case ConsoleKey.UpArrow:
                    return input.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionUp : EngineInput.DirectionUp;
                case ConsoleKey.NumPad9:
                    return input.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionUpRight : EngineInput.DirectionUpRight;
                case ConsoleKey.NumPad6:
                case ConsoleKey.RightArrow:
                    return input.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionRight : EngineInput.DirectionRight;
                case ConsoleKey.NumPad3:
                    return input.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionDownRight : EngineInput.DirectionDownRight;
                case ConsoleKey.NumPad2:
                case ConsoleKey.DownArrow:
                    return input.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionDown : EngineInput.DirectionDown;
                case ConsoleKey.NumPad1:
                    return input.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionDownLeft : EngineInput.DirectionDownLeft;
                case ConsoleKey.NumPad4:
                case ConsoleKey.LeftArrow:
                    return input.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionLeft : EngineInput.DirectionLeft;
                case ConsoleKey.NumPad7:
                    return input.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionUpLeft : EngineInput.DirectionUpLeft;
                case ConsoleKey.NumPad5:
                    return input.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionCenter : EngineInput.DirectionCenter;
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
            Console.BackgroundColor = DefaultTile.BackgroundColor;
            Console.ForegroundColor = DefaultTile.ForegroundColor;
            Console.Clear();
        }

        protected override async Task DrawScreenTileAsync(int x, int y)
        {
            Console.CursorVisible = false;
            if (x < Console.BufferWidth && y < Console.BufferHeight)
            {
                Console.SetCursorPosition(x, y);

                var tile = TileBuffer[x, y] ?? DefaultTile;

                Console.BackgroundColor = tile.BackgroundColor;
                Console.ForegroundColor = tile.ForegroundColor;
                Console.Write(tile.Char);
            }
        }
    }
}
