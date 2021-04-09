// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Kolohe
{
    public class Map
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

        private readonly MapTile[,] _tiles = new MapTile[MapWidth, MapHeight];

        public const int MapWidth = 20;
        public const int MapHeight = 10;

        public bool Within(int x, int y)
        {
            return x >= 0 && x < MapWidth && y >= 0 && y < MapHeight;
        }

        public static Map GetStaticMap()
        {
            var map = new Map();

            var innerRect = new Rect(2, 2, 4, 4);

            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    if (x == 0 || x == MapWidth - 1 || y == 0 || y == MapHeight - 1)
                    {
                        map[x, y] = MapTile.Wall;
                    }
                    else if (innerRect.PointOnEdge(x, y))
                    {
                        map[x, y] = MapTile.Wall;
                    }
                    else if (!innerRect.Within(x, y))
                    {
                        map[x, y] = MapTile.Floor;
                    }
                }
            }

            return map;
        }
    }
}
