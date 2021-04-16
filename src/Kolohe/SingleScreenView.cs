// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Kolohe
{
    public abstract class SingleScreenView<T> : IView where T : class, ISingleScreenTile
    {
        public Rect ScreenBounds { get; private set; }

        public Rect MapWindow { get; private set; }

        public T?[,] TileBuffer { get; protected set; }

        public SingleScreenView(int width, int height)
        {
            ScreenBounds = new Rect(width, height);
            MapWindow = new Rect(width, height);
            TileBuffer = new T[width, height];
        }

        public void Resize(int width, int height)
        {
            ScreenBounds = new Rect(width, height);
            MapWindow = new Rect(width, height);
            TileBuffer = new T[width, height];
        }

        public virtual async Task<EngineInput> ReadInputAsync()
        {
            await Task.Yield();
            return EngineInput.None;
        }

        public async Task UpdateViewAsync(Engine engine, EngineInput input)
        {
            bool forceRefresh = SyncScreenDimensions() || input == EngineInput.RefreshView;

            if (forceRefresh)
            {
                await ClearScreenAsync();
            }

            for (int x = 0; x < ScreenBounds.Width; x++)
            {
                for (int y = 0; y < ScreenBounds.Height; y++)
                {
                    var oldTile = TileBuffer[x, y];
                    var newTile = TileBuffer[x, y];

                    if (MapWindow.Contains(x, y))
                    {
                        // Draw Map
                        int xOffset = 0;
                        int yOffset = 0;

                        int mx = x + xOffset;
                        int my = y + yOffset;

                        newTile = engine.Map.Contains(mx, my) ? GetTile(engine.Map[mx, my], mx == engine.Player.X && my == engine.Player.Y) : null;
                    }
                    
                    TileBuffer[x, y] = newTile;

                    if ((oldTile is not null && !oldTile.Equals(newTile)) || forceRefresh)
                    {
                        await DrawTileAsync(x, y);
                    }
                }
            }
        }

        protected virtual bool SyncScreenDimensions()
        {
            return false;
        }

        protected virtual T? GetTile(MapTile mapTile, bool player)
        {
            return null;
        }

        protected virtual async Task ClearScreenAsync()
        {
            await Task.Yield();
        }

        protected virtual async Task DrawTileAsync(int x, int y)
        {
            await Task.Yield();
        }
    }
}
