// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Kolohe
{
    public class ConsoleTile : ISingleScreenTile
    {
        public char Char = ' ';
        public ConsoleColor BackgroundColor = ConsoleColor.Black;
        public ConsoleColor ForegroundColor = ConsoleColor.White;

        public override bool Equals(object? obj)
        {
            if (obj is ConsoleTile ct)
            {
                return Char == ct.Char && BackgroundColor == ct.BackgroundColor && ForegroundColor == ct.ForegroundColor;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Char, BackgroundColor, ForegroundColor);
        }
    }
}