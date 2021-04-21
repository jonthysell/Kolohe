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
                    map[x, y] = MapTile.SaltWater;
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
                    isClear = isClear && map[x, y] == MapTile.SaltWater;
                });

                if (isClear)
                {
                    var landNoise = MultiLevelNoise.Generate(random, 8);
                    var waterNoise = MultiLevelNoise.Generate(random, 8);

                    (int centerX, int centerY) = islandRect.GetCenter();
                    double islandRadius = Math.Min(islandRect.Width, islandRect.Height) / 2.0;

                    islandRect.ForEach((x, y) =>
                    {
                        // Get average height of surrounding areas to smooth things out
                        double landHeight = landNoise.Noise2(x, y) / ((int)Direction.NumDirections + 1);
                        double waterAmount = waterNoise.Noise2(x, y) / ((int)Direction.NumDirections + 1);
                        DirectionExtensions.ForEachDirection(x, y, (dx, dy) =>
                        {
                            landHeight += landNoise.Noise2(dx, dy) / ((int)Direction.NumDirections + 1);
                            waterAmount += waterNoise.Noise2(dx, dy) / ((int)Direction.NumDirections + 1);
                        });

                        landHeight = MathExt.RemapValue(landHeight, -1, 1, 0, 1);
                        waterAmount = MathExt.RemapValue(waterAmount, -1, 1, 0, 1);

                        // Apply radial gradient to consolidate land in the middle
                        double radialGradient = Math.Pow(MathExt.GetDistance(x, y, centerX, centerY) / islandRadius, 2);
                        landHeight *= Math.Max(0, 1.0 - radialGradient);
                        waterAmount *= Math.Max(0, 1.0 - radialGradient);

                        // Map height to land-based tiles
                        map[x, y] = (MapTile)MathExt.RemapValue(landHeight, 0, 1, (int)MapTile.FreshWater, (int)MapTile.Rock);
                    });

                    MathExt.Fill(islandRect.X, islandRect.Y, MapTile.SaltWater, (x, y) => map[x, y], (x, y, val) => map[x, y] = val);

                    return true;
                }
            }

            return false;
        }
    }
}
