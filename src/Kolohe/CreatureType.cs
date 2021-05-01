// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Kolohe
{
    public enum CreatureType
    {
        Player,
        Bird,
        Fish,
    }

    public static class CreatureTypeExtensions
    {
        public static bool CanExistOnMapTile(this CreatureType creatureType, MapTile mapTile)
        {
            switch (creatureType)
            {
                case CreatureType.Player:
                case CreatureType.Bird:
                    return mapTile.IsLand() || mapTile.IsWater();
                case CreatureType.Fish:
                    return mapTile.IsWater();
            }

            return false;
        }
    }
}
