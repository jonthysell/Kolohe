// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Kolohe
{
    public abstract class SingleScreenView<T> : IView where T : ISingleScreenTile
    {
        public int ScreenWidth { get; private set; }

        public int ScreenHeight { get; private set; }

        public T?[,] TileBuffer { get; protected set; }

        public SingleScreenView(int width, int height)
        {
            Resize(width, height);
        }

        public void Resize(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            TileBuffer = new T?[width, height];
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
                        var oldTile = TileBuffer[x, y];

                        var newTile = GetTile(engine.Map[x, y], x == engine.Player.X && y == engine.Player.Y);

                        TileBuffer[x, y] = newTile;

                        if (oldTile is null || !oldTile.Equals(newTile) || forceRefresh)
                        {
                            await DrawTile(x, y);
                        }
                    }
                }
            }
        }

        protected virtual bool SyncScreenDimensions() => false;

        protected virtual T GetTile(MapTile mapTile, bool player) => default;

        protected virtual async Task DrawTile(int x, int y) { }

        
    }
}
