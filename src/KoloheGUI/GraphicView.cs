// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Collections.Concurrent;
using System.Threading;
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

        private readonly ConcurrentQueue<KeyEventArgs> _inputQueue = new ConcurrentQueue<KeyEventArgs>();

        public GraphicView(MainWindow mainWindow, int width, int height) : base(width, height)
        {
            _mainWindow = mainWindow;
            _mainWindow.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object? sender, KeyEventArgs e)
        {
            _inputQueue.Enqueue(e);
        }

        public override async Task<EngineInput> ReadInputAsync(CancellationToken token)
        {
            return await Task.Run(async () =>
            {
                KeyEventArgs? input = null;
                while (!token.IsCancellationRequested && !_inputQueue.TryDequeue(out input))
                {
                    await Task.Yield();
                }

                if (input is not null)
                {
                    switch (input.Key)
                    {
                        default:
                            return EngineInput.HaltEngine;
                    }
                }

                return EngineInput.None;
            });
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
