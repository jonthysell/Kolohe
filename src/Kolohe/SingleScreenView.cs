// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Kolohe
{
    public abstract class SingleScreenView<T> : IView where T : class, ISingleScreenTile
    {
        public int ScreenWidth { get; private set; }

        public int ScreenHeight { get; private set; }

        public T?[,] TileBuffer { get; protected set; }

        public SingleScreenView(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            TileBuffer = new T[width, height];
        }

        public void Resize(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
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

            for (int x = 0; x < ScreenWidth; x++)
            {
                for (int y = 0; y < ScreenHeight; y++)
                {
                    if (x < engine.Map.Width && y < engine.Map.Height)
                    {
                        var oldTile = TileBuffer[x, y];

                        var newTile = engine.Map.Contains(x, y) ? GetTile(engine.Map[x, y], x == engine.Player.X && y == engine.Player.Y) : null;

                        TileBuffer[x, y] = newTile;

                        if (oldTile is null || !oldTile.Equals(newTile) || forceRefresh)
                        {
                            await DrawTileAsync(x, y);
                        }
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
