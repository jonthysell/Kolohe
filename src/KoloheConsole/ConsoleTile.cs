// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Kolohe
{
    public class ConsoleTile : ISingleScreenTile, IEquatable<ConsoleTile>
    {
        public char Char = ' ';
        public ConsoleColor BackgroundColor = ConsoleColor.Black;
        public ConsoleColor ForegroundColor = ConsoleColor.White;

        public static int eq = 0;
        public static int op = 0;

        public bool Equals(ConsoleTile? other)
        {
            return other is not null && Char == other.Char && BackgroundColor == other.BackgroundColor && ForegroundColor == other.ForegroundColor;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ConsoleTile);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Char, BackgroundColor, ForegroundColor);
        }
    }
}