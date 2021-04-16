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
                    map[x, y] = MapTile.Ocean;
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
                    isClear = isClear && map[x, y] == MapTile.Ocean;
                });

                if (isClear)
                {
                    islandRect.ForEach((x, y) =>
                    {
                        map[x, y] = MapTile.Sand;
                    });
                    return true;
                }
            }

            return false;
        }
    }
}
