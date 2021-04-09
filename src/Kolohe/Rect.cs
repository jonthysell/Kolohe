// Copyright(c) Jon Thysell<http://jonthysell.com>
// Licensed under the MIT License.

namespace Kolohe
{
    public readonly struct Rect
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool WithinWidth(int x)
        {
            return x >= X && x < (X + Width);
        }

        public bool LeftEdge(int x)
        {
            return x == X;
        }

        public bool RightEdge(int x)
        {
            return x == X + Width - 1;
        }

        public bool TopEdge(int y)
        {
            return y == Y;
        }

        public bool BottomEdge(int y)
        {
            return y == Y + Height - 1;
        }

        public bool WithinHeight(int y)
        {
            return y >= Y && y < (Y + Height);
        }

        public bool Within(int x, int y)
        {
            return WithinWidth(x) && WithinHeight(y);
        }

        public bool PointOnEdge(int x, int y)
        {
            return ((LeftEdge(x) || RightEdge(x)) && WithinHeight(y))
                || ((TopEdge(y) || BottomEdge(y)) && WithinWidth(x));
        }

        public bool PointOnCorner(int x, int y)
        {
            return (LeftEdge(x) || RightEdge(x)) && (TopEdge(y) || BottomEdge(y));
        }
    }
}
