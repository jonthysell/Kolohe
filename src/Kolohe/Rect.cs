// Copyright(c) Jon Thysell<http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Kolohe
{
    public class Rect
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public int MaxX => X + Width - 1;
        public int MaxY => Y + Height - 1;

        public int Area => Width * Height;

        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width > 0 ? width : throw new ArgumentOutOfRangeException(nameof(width));
            Height = height > 0 ? height : throw new ArgumentOutOfRangeException(nameof(height));
        }

        public Rect(int width, int height) : this(0, 0, width, height) { }

        public bool Contains(int x, int y)
        {
            return x >= X && x <= MaxX && y >= Y && y <= MaxY;
        }

        public bool Contains(Rect rect)
        {
            return Contains(rect.X, rect.Y) && Contains(rect.MaxX, rect.MaxY);
        }

        public void ForEach(Action<int, int> action)
        {
            for (int x = X; x <= MaxX; x++)
            {
                for (int y = Y; y <= MaxY; y++)
                {
                    action(x, y);
                }
            }
        }

        public Rect Grow(int amount)
        {
            return new Rect(X - amount, Y - amount, Width + (2 * amount), Height + (2 * amount));
        }

        public Rect Shrink(int amount)
        {
            return new Rect(X + amount, Y + amount, Width - (2 * amount), Height - (2 * amount));
        }

        public static Rect RandomRect(Random random, int minX, int maxX, int minY, int maxY, int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            int x = random.Next(minX, maxX + 1);
            int y = random.Next(minY, maxY + 1);

            int width = random.Next(minWidth, maxWidth + 1);
            int height = random.Next(minHeight, maxHeight + 1);

            return new Rect(x, y, width, height);
        }
    }
}
