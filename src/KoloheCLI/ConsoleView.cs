// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kolohe.CLI
{
    class ConsoleView : SingleScreenView<ConsoleTile>
    {
        public readonly ConsoleTile DefaultTile = new ConsoleTile();

        public ConsoleView() : base(Console.WindowWidth, Console.WindowHeight) { }

        public async Task ReadKeysAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                while (!Console.KeyAvailable)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    await Task.Yield();
                }

                var input = EngineInput.None;

                var cki = Console.ReadKey(true);

                switch (cki.Key)
                {
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.UpArrow:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionUp : EngineInput.DirectionUp;
                        break;
                    case ConsoleKey.NumPad9:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionUpRight : EngineInput.DirectionUpRight;
                        break;
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.RightArrow:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionRight : EngineInput.DirectionRight;
                        break;
                    case ConsoleKey.NumPad3:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionDownRight : EngineInput.DirectionDownRight;
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.DownArrow:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionDown : EngineInput.DirectionDown;
                        break;
                    case ConsoleKey.NumPad1:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionDownLeft : EngineInput.DirectionDownLeft;
                        break;
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.LeftArrow:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionLeft : EngineInput.DirectionLeft;
                        break;
                    case ConsoleKey.NumPad7:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionUpLeft : EngineInput.DirectionUpLeft;
                        break;
                    case ConsoleKey.NumPad5:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.ModifiedDirectionCenter : EngineInput.DirectionCenter;
                        break;
                    case ConsoleKey.R:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.RefreshView : EngineInput.None;
                        break;
                    case ConsoleKey.C:
                        input = cki.Modifiers.HasFlag(ConsoleModifiers.Control) ? EngineInput.HaltEngine : EngineInput.None;
                        break;
                }

                InputBuffer.Enqueue(input);
            }
        }

        protected override async Task<bool> SyncScreenDimensionsAsync()
        {
            return await Task.Run(() =>
            {
                bool refresh = false;
                if (Console.WindowWidth != ScreenBounds.Width || Console.WindowHeight != ScreenBounds.Height)
                {
                    Resize(Console.WindowWidth, Console.WindowHeight);
                    refresh = true;
                }

                if (Console.WindowWidth != Console.BufferWidth || Console.WindowHeight != Console.BufferHeight)
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    Console.SetWindowPosition(0, 0);
                    Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
#pragma warning restore CA1416 // Validate platform compatibility
                    refresh = true;
                }

                return refresh;
            });
        }

        protected override ConsoleTile GetMapTile(MapTile mapTile, bool player)
        {
            var consoleTile = new ConsoleTile();

            switch (mapTile)
            {
                case MapTile.OpenOcean:
                    consoleTile.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case MapTile.Water:
                    consoleTile.BackgroundColor = ConsoleColor.Blue;
                    break;
                case MapTile.Sand:
                    consoleTile.Char = BlockChars.LightShade;
                    consoleTile.BackgroundColor = ConsoleColor.Yellow;
                    consoleTile.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case MapTile.Dirt:
                    consoleTile.Char = BlockChars.MediumShade;
                    consoleTile.BackgroundColor = ConsoleColor.Yellow;
                    consoleTile.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case MapTile.Rock:
                    consoleTile.Char = BlockChars.DarkShade;
                    consoleTile.BackgroundColor = ConsoleColor.Yellow;
                    consoleTile.ForegroundColor = ConsoleColor.DarkGray;
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
            await Task.Run(() =>
            {
                Console.BackgroundColor = DefaultTile.BackgroundColor;
                Console.ForegroundColor = DefaultTile.ForegroundColor;
                Console.Clear();
            });
        }

        protected override IEnumerable<(int, int)> GetScreenCoordinatesEnumerable()
        {
            // Some consoles get weird artifacts when rendering top down so we work
            // around it by rendering bottom up
            for (int y = ScreenBounds.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < ScreenBounds.Width; x++)
                {
                    yield return (x, y);
                }
            }
        }

        protected override async Task DrawScreenTileAsync(int x, int y)
        {
            await Task.Run(() =>
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
            });
        }
    }
}
