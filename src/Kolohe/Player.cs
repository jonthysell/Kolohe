// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

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

        public static Player GeneratePlayer(Random random)
        {
            return new Player();
        }
    }
}
