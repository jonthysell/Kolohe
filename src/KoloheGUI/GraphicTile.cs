// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using Avalonia.Media;

namespace Kolohe.GUI
{
    public class GraphicTile : ISingleScreenTile, IEquatable<GraphicTile>
    {
        public char Char = ' ';
        public Color BackgroundColor = Colors.Black;
        public Color ForegroundColor = Colors.White;

        public bool Equals(GraphicTile? other)
        {
            return other is not null && Char == other.Char && BackgroundColor == other.BackgroundColor && ForegroundColor == other.ForegroundColor;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as GraphicTile);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Char, BackgroundColor, ForegroundColor);
        }
    }
}