// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Kolohe
{
    public abstract class SingleScreenView : IView
    {
        public int ScreenWidth { get; private set; }

        public int ScreenHeight { get; private set; }

        public SingleScreenView(int width, int height)
        {
            Resize(width, height);
        }

        public virtual void Resize(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
        }

        public virtual async Task<EngineInput> ReadInputAsync() => EngineInput.None;

        public async Task UpdateViewAsync(Engine engine, EngineInput input)
        {
            bool forceRefresh = SyncScreenDimensions() || input == EngineInput.RefreshView;

            for (int x = 0; x < ScreenWidth; x++)
            {
                for (int y = 0; y < ScreenHeight; y++)
                {
                    if (x < Map.MapWidth && y < Map.MapHeight)
                    {
                        await DrawMapTileAsync(x, y, engine.Map[x, y], forceRefresh);
                        if (x == engine.Player.X && y == engine.Player.Y)
                        {
                            await DrawEntityAsync(x, y, forceRefresh);
                        }
                    }
                }
            }
        }

        protected virtual bool SyncScreenDimensions() => false;

        protected virtual async Task DrawMapTileAsync(int x, int y, MapTile tile, bool forceRefresh) { }

        protected virtual async Task DrawEntityAsync(int x, int y, bool forceRefresh) { }
    }
}
