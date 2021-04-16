// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Kolohe
{
    public enum Direction
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft,
        NumDirections
    }

    public static class DirectionExtensions
    {
        public static (int dx, int dy) GetDeltas(this Direction direction)
        {
            return (_directionDeltas[(int)direction % (int)Direction.NumDirections][0], _directionDeltas[(int)direction % (int)Direction.NumDirections][1]);
        }

        private readonly static int[][] _directionDeltas = new int[][]
        {
            new int[] { 0, -1 },
            new int[] { 1, -1 },
            new int[] { 1, 0 },
            new int[] { 1, 1 },
            new int[] { 0, 1 },
            new int[] { -1, 1 },
            new int[] { -1, 0 },
            new int[] { -1, -1 },
        };
    }
}
