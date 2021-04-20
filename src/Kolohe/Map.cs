// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Kolohe
{
    public class Map : Rect
    {
        public MapTile this[int x, int y]
        {
            get
            {
                return _tiles[x, y];
            }
            internal set
            {
                _tiles[x, y] = value;
            }
        }

        private readonly MapTile[,] _tiles;

        public Map(int width, int height) : base(width, height)
        {
            _tiles = new MapTile[width, height];
        }

        public static Map GenerateWorldMap(Random random)
        {
            var map = new Map(Constants.WorldMapWidth, Constants.WorldMapHeight);

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    map[x, y] = MapTile.OpenOcean;
                }
            }

            int islands = 0;
            while (islands < Constants.WorldIslandCount)
            {
                if (TryAddIsland(map, random))
                {
                    islands++;
                }
            }

            return map;
        }

        private static bool TryAddIsland(Map map, Random random)
        {
            var islandRect = RandomRect(random, 0, map.Width, 0, map.Height, Constants.MinIslandSize, Constants.MaxIslandSize, Constants.MinIslandSize, Constants.MaxIslandSize);
            var bufferRect = islandRect.ResizeFromCenter(Constants.MinIslandDistance);

            if (map.Contains(bufferRect))
            {
                bool isClear = true;
                bufferRect.ForEach((x, y) =>
                {
                    isClear = isClear && map[x, y] == MapTile.OpenOcean;
                });

                if (isClear)
                {
                    var openSimplex = new OpenSimplex2F(random.Next());

                    (int centerX, int centerY) = islandRect.GetCenter();
                    double islandRadius = Math.Min(islandRect.Width, islandRect.Height) / 3.0;

                    islandRect.ForEach((x, y) =>
                    {
                        // Get average height of surrounding areas to smooth things out
                        double height = openSimplex.Noise2(x, y) / ((int)Direction.NumDirections + 1);
                        DirectionExtensions.ForEachDirection(x, y, (dx, dy) =>
                        {
                            height += openSimplex.Noise2(dx, dy) / ((int)Direction.NumDirections + 1);
                        });

                        height = MathExt.RemapValue(height, -1, 1, 0, 1);

                        // Apply logarithmic radial gradient to consolidate land in the middle
                        double radialGradient = Math.Log(MathExt.GetDistance(x, y, centerX, centerY) / islandRadius);
                        height -= radialGradient;
                        height = Math.Clamp(height, 0, 1);

                        // Map height to tiles
                        map[x, y] = (MapTile)Math.Round(MathExt.RemapValue(height, 0, 1, (int)MapTile.OpenOcean, (int)MapTile.Rock));
                    });

                    return true;
                }
            }

            return false;
        }
    }
}
