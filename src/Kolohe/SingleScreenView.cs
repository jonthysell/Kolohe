// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Kolohe
{
    public abstract class SingleScreenView<T> : IView where T : class, ISingleScreenTile, IEquatable<T>
    {
        public Rect ScreenBounds { get; private set; }

        public Rect MapWindow { get; private set; }

        public int MapCameraXOffset { get; private set; } = 0;

        public int MapCameraYOffset { get; private set; } = 0;

        public T?[,] TileBuffer { get; protected set; }

        public SingleScreenView(int width, int height)
        {
            ScreenBounds = new Rect(width, height);
            MapWindow = ScreenBounds.ResizeFromCenter(-1);
            TileBuffer = new T?[width, height];
        }

        public void Resize(int width, int height)
        {
            ScreenBounds = new Rect(width, height);
            MapWindow = ScreenBounds.ResizeFromCenter(-1);
            TileBuffer = new T?[width, height];
        }

        public void MoveMapCamera(Direction direction)
        {
            (int dx, int dy) = direction.GetDeltas();
            MapCameraXOffset += dx;
            MapCameraYOffset += dy;
        }

        public void CenterMapCamera(int mapX, int mapY)
        {
            MapCameraXOffset = mapX - (MapWindow.Width / 2);
            MapCameraYOffset = mapY - (MapWindow.Height / 2);
        }

        public virtual async Task<EngineInput> ReadInputAsync()
        {
            await Task.Yield();
            return EngineInput.None;
        }

        public async Task UpdateViewAsync(Engine engine, EngineInput input)
        {
            // Re-sync the screen
            bool forceRefresh = SyncScreenDimensions() || input == EngineInput.RefreshView;

            // Process input to re-center map
            switch (input)
            {
                case EngineInput.ModifiedDirectionUp:
                case EngineInput.ModifiedDirectionUpRight:
                case EngineInput.ModifiedDirectionRight:
                case EngineInput.ModifiedDirectionDownRight:
                case EngineInput.ModifiedDirectionDown:
                case EngineInput.ModifiedDirectionDownLeft:
                case EngineInput.ModifiedDirectionLeft:
                case EngineInput.ModifiedDirectionUpLeft:
                    MoveMapCamera((Direction)((int)input - (int)EngineInput.ModifiedDirectionUp));
                    break;
                case EngineInput.ModifiedDirectionCenter:
                    CenterMapCamera(engine.Player.X, engine.Player.Y);
                    break;
            }

            // Move camera to keep player on screen
            var playerBounds = MapWindow.Shift(MapCameraXOffset, MapCameraYOffset).ResizeFromCenter(-1);
            if (playerBounds.GetRectPart(engine.Player.X, engine.Player.Y) != RectPart.Inside)
            {
                // TODO: Centering on player is aggressive but fine for now, replace with smoother pan
                CenterMapCamera(engine.Player.X, engine.Player.Y);
            }

            if (forceRefresh)
            {
                await ClearScreenAsync();
            }

            for (int screenY = ScreenBounds.Height - 1; screenY >= 0; screenY--)
            {
                for (int screenX = 0; screenX < ScreenBounds.Width; screenX++)
                {
                    T? oldTile = TileBuffer[screenX, screenY];
                    T? newTile = null;

                    if (MapWindow.Contains(screenX, screenY))
                    {
                        // Get map tile
                        int mapX = screenX - MapWindow.X + MapCameraXOffset;
                        int mapY = screenY - MapWindow.Y + MapCameraYOffset;

                        newTile = engine.Map.Contains(mapX, mapY) ? GetMapTile(engine.Map[mapX, mapY], mapX == engine.Player.X && mapY == engine.Player.Y) : null;
                    }

                    if (forceRefresh || (oldTile is not null && !oldTile.Equals(newTile)) || (newTile is not null && !newTile.Equals(oldTile)))
                    {
                        TileBuffer[screenX, screenY] = newTile;
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
