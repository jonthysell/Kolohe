// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

using Avalonia.Media;

namespace Kolohe.GUI
{
    public class GraphicView : SingleScreenView<GraphicTile>
    {
        public readonly GraphicTile DefaultTile = new GraphicTile();

        public GraphicView(int width, int height) : base(width, height) { }

        public override async Task<EngineInput> ReadInputAsync()
        {
            return await Task.Run(() =>
            {
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
