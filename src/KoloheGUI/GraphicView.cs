// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace Kolohe.GUI
{
    public class GraphicView : SingleScreenView<GraphicTile>
    {
        public readonly GraphicTile DefaultTile = new GraphicTile();

        public readonly MainWindow Window;

        public TileControl?[,] TileControls { get; protected set; } 

        public GraphicView(MainWindow window, int width, int height) : base(width, height)
        {
            TileControls = new TileControl?[width, height];
            Window = window;
            Window.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object? sender, KeyEventArgs e)
        {
            var input = EngineInput.None;
            switch (e.Key)
            {
                case Key.NumPad8:
                case Key.Up:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.ModifiedDirectionUp : EngineInput.DirectionUp;
                    break;
                case Key.NumPad9:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.ModifiedDirectionUpRight : EngineInput.DirectionUpRight;
                    break;
                case Key.NumPad6:
                case Key.Right:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.ModifiedDirectionRight : EngineInput.DirectionRight;
                    break;
                case Key.NumPad3:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.ModifiedDirectionDownRight : EngineInput.DirectionDownRight;
                    break;
                case Key.NumPad2:
                case Key.Down:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.ModifiedDirectionDown : EngineInput.DirectionDown;
                    break;
                case Key.NumPad1:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.ModifiedDirectionDownLeft : EngineInput.DirectionDownLeft;
                    break;
                case Key.NumPad4:
                case Key.Left:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.ModifiedDirectionLeft : EngineInput.DirectionLeft;
                    break;
                case Key.NumPad7:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.ModifiedDirectionUpLeft : EngineInput.DirectionUpLeft;
                    break;
                case Key.NumPad5:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.ModifiedDirectionCenter : EngineInput.DirectionCenter;
                    break;
                case Key.R:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.RefreshView : EngineInput.None;
                    break;
                case Key.C:
                    input = e.KeyModifiers.HasFlag(KeyModifiers.Control) ? EngineInput.HaltEngine : EngineInput.None;
                    break;
            }
            InputBuffer.Enqueue(input);
        }

        public override void Resize(int width, int height)
        {
            base.Resize(width, height);
            TileControls = new TileControl?[width, height];
        }

        protected override async Task<bool> SyncScreenDimensionsAsync()
        {
            bool refresh = false;

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                int expectedWidth = (int)(Window.ScreenCanvas?.Bounds.Width ?? 0) / TileControl.TileWidth;
                int expectedHeight = (int)(Window.ScreenCanvas?.Bounds.Height ?? 0) / TileControl.TileHeight;

                if (expectedWidth != ScreenBounds.Width || expectedHeight != ScreenBounds.Height)
                {
                    Resize(expectedWidth, expectedHeight);
                    refresh = true;
                }
            });

            return refresh;
        }

        protected override GraphicTile GetMapTile(MapTile mapTile, CreatureType? creatureType)
        {
            var tile = new GraphicTile();

            switch (mapTile)
            {
                case MapTile.SaltWater:
                    tile.BackgroundColor = Colors.DarkBlue;
                    break;
                case MapTile.FreshWater:
                    tile.BackgroundColor = Colors.Cyan;
                    break;
                case MapTile.Sand:
                    tile.Char = BlockChars.LightShade;
                    tile.BackgroundColor = Colors.Yellow;
                    tile.ForegroundColor = Colors.SandyBrown;
                    break;
                case MapTile.Dirt:
                    tile.Char = BlockChars.MediumShade;
                    tile.BackgroundColor = Colors.Yellow;
                    tile.ForegroundColor = Colors.SandyBrown;
                    break;
                case MapTile.Grass:
                    tile.Char = BlockChars.LightShade;
                    tile.BackgroundColor = Colors.Green;
                    tile.ForegroundColor = Colors.DarkGreen;
                    break;
                case MapTile.Rock:
                    tile.Char = BlockChars.DarkShade;
                    tile.BackgroundColor = Colors.Gray;
                    tile.ForegroundColor = Colors.DarkGray;
                    break;
                default:
                    return new GraphicTile();
            }

            if (creatureType.HasValue)
            {
                switch (creatureType)
                {
                    case CreatureType.Player:
                        tile.Char = '@';
                        tile.ForegroundColor = Colors.White;
                        break;
                }
            }

            return tile;
        }

        protected override async Task ClearScreenAsync()
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Window.ScreenCanvas?.Children.Clear();
            });
            for (int y = 0; y < ScreenBounds.Height; y++)
            {
                for (int x = 0; x < ScreenBounds.Width; x++)
                {
                    TileControls[x, y] = null;
                }
            }
        }

        protected override async Task DrawScreenTileAsync(int x, int y)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var tile = TileBuffer[x, y] ??= DefaultTile;

                if (TileControls[x, y] is null)
                {
                    var tileControl = new TileControl();
                    tileControl.Update(x, y);
                    var canvas = Window.ScreenCanvas;
                    if (canvas is not null)
                    {
                        tileControl.Update(x, y);
                        canvas.Children.Add(tileControl);
                    }
                    TileControls[x, y] = tileControl;
                }

                TileControls[x, y]?.Update(tile);                
            });
        }
    }
}
