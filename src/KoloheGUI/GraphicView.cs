// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace Kolohe.GUI
{
    public class GraphicView : SingleScreenView<GraphicTile>
    {
        public readonly GraphicTile DefaultTile = new GraphicTile();

        private readonly MainWindow _mainWindow;

        public GraphicView(MainWindow mainWindow, int width, int height) : base(width, height)
        {
            _mainWindow = mainWindow;
            _mainWindow.KeyDown += MainWindow_KeyDown;
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

        protected override bool SyncScreenDimensions()
        {
            bool refresh = false;

            return refresh;
        }

        protected override GraphicTile GetMapTile(MapTile mapTile, bool player)
        {
            var consoleTile = new GraphicTile();

            switch (mapTile)
            {
                case MapTile.Ocean:
                    consoleTile.BackgroundColor = Colors.DarkBlue;
                    break;
                case MapTile.Sand:
                    consoleTile.Char = BlockChars.LightShade;
                    consoleTile.BackgroundColor = Colors.Yellow;
                    consoleTile.ForegroundColor = Colors.SandyBrown;
                    break;
                default:
                    return new GraphicTile();
            }

            if (player)
            {
                consoleTile.Char = '@';
                consoleTile.ForegroundColor = Colors.Green;
            }

            return consoleTile;
        }

        protected override async Task ClearScreenAsync()
        {
            await Task.Run(() =>
            {
                
            });
        }

        protected override async Task DrawScreenTileAsync(int x, int y)
        {
            await Task.Run(() =>
            {
                
            });
        }
    }
}
