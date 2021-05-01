// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Kolohe
{
    public class Player : Creature
    {
        public override CreatureType Type => CreatureType.Player;

        public Player()
        {
            X = -1;
            Y = -1;
        }
    }
}
