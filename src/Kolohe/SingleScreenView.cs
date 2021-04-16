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
            MapWindow = ScreenBounds.Shrink(1);
            TileBuffer = new T[width, height];
        }

        public void Resize(int width, int height)
        {
            ScreenBounds = new Rect(width, height);
            MapWindow = ScreenBounds.Shrink(1);
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

            for (int screenY = 0; screenY < ScreenBounds.Height; screenY++)
            {
                for (int screenX = 0; screenX < ScreenBounds.Width; screenX++)
                {
                    var oldTile = TileBuffer[screenX, screenY];
                    var newTile = TileBuffer[screenX, screenY];

                    if (MapWindow.Contains(screenX, screenY))
                    {
                        // Draw Map
                        int mx = screenX - MapWindow.X;
                        int my = screenY - MapWindow.Y;

                        newTile = engine.Map.Contains(mx, my) ? GetMapTile(engine.Map[mx, my], mx == engine.Player.X && my == engine.Player.Y) : null;
                    }
                    
                    TileBuffer[screenX, screenY] = newTile;

                    if ((oldTile is not null && !oldTile.Equals(newTile)) || forceRefresh)
                    {
                        await DrawScreenTileAsync(screenX, screenY);
                    }
                }
            }
        }

        protected virtual bool SyncScreenDimensions()
        {
            return false;
        }

        protected virtual T? GetMapTile(MapTile mapTile, bool player)
        {
            return null;
        }

        protected virtual async Task ClearScreenAsync()
        {
            await Task.Yield();
        }

        protected virtual async Task DrawScreenTileAsync(int x, int y)
        {
            await Task.Yield();
        }
    }
}
