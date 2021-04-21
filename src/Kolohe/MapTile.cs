// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Kolohe
{
    public enum MapTile
    {
        None,
        SaltWater,
        FreshWater,
        Sand,
        Dirt,
        Rock,
        Grass,
    }

    public static class MapTileExtensions
    {
        public static bool IsLand(this MapTile mapTile)
        {
            switch (mapTile)
            {
                case MapTile.Sand:
                case MapTile.Dirt:
                case MapTile.Rock:
                case MapTile.Grass:
                    return true;
            }

            return false;
        }

        public static bool IsWater(this MapTile mapTile)
        {
            switch (mapTile)
            {
                case MapTile.SaltWater:
                case MapTile.FreshWater:
                    return true;
            }

            return false;
        }
    }
}
