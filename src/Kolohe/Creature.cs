// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Kolohe
{
    public abstract class Creature : IEntity
    {
        public int X { get; set; }
        public int Y { get; set; }

        public abstract CreatureType Type { get; }

        public bool CanPlaceOnTile(MapTile mapTile)
        {
            return Type.CanExistOnMapTile(mapTile);
        }
    }
}
