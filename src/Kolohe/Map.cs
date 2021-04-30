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
                    var landNoise = MultiLevelNoise.Generate(random, 8, 0, 1, 4, 0.25);
                    var waterNoise = MultiLevelNoise.Generate(random, 8, 0, 1, 4, 0.25);

                    (int centerX, int centerY) = islandRect.GetCenter();
                    double islandRadius = Math.Min(islandRect.Width, islandRect.Height) / 2.0;

                    islandRect.ForEach((x, y) =>
                    {
                        // Get average height of surrounding areas to smooth things out
                        double landHeight = landNoise.Noise2(x, y) / ((int)Direction.NumDirections + 1);
                        double waterAmount = waterNoise.Noise2(x, y) / ((int)Direction.NumDirections + 1);
                        DirectionExtensions.ForEachDirection(x, y, (x2, y2) =>
                        {
                            landHeight += landNoise.Noise2(x2, y2) / ((int)Direction.NumDirections + 1);
                            waterAmount += waterNoise.Noise2(x2, y2) / ((int)Direction.NumDirections + 1);
                        });

                        // Apply radial gradient to consolidate land in the middle
                        double radialGradient = Math.Pow(1.0 - Math.Clamp(MathExt.GetDistance(x, y, centerX, centerY) / islandRadius, 0, 1), 2);
                        landHeight *= radialGradient;
                        waterAmount *= radialGradient;

                        // Map height to land-based tiles
                        var tile = MapTile.FreshWater;
                        if (landHeight < 0.05)
                        {
                            tile = MapTile.FreshWater;
                        }
                        else if (landHeight < 0.10)
                        {
                            tile = MapTile.Sand;
                        }
                        else if (landHeight < 0.15)
                        {
                            tile = MapTile.Dirt;
                        }
                        else if (landHeight < 0.50)
                        {
                            tile = waterAmount > 0.30 && waterAmount < 0.35 ? MapTile.FreshWater : MapTile.Grass;
                        }
                        else
                        {
                            tile = MapTile.Rock;
                        }
                        map[x, y] = tile;
                    });

                    MathExt.Fill(islandRect.X, islandRect.Y, MapTile.SaltWater, (x, y) => map[x, y], (x, y, val) => map[x, y] = val);

                    return true;
                }
            }

            return false;
        }
    }
}
