// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Kolohe
{
    public class Player : IEntity
    {
        public int X;
        public int Y;

        public bool CanTravelOnTile(MapTile mapTile)
        {
            return mapTile.IsLand() || mapTile.IsWater();
        }
    }
}
