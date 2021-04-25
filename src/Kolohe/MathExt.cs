// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Kolohe
{
    public static class MathExt
    {
        public static double RemapValue(double value, double inMin, double inMax, double outMin, double outMax, bool clamp = true)
        {
            var outValue = outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
            return clamp ? Math.Clamp(outValue, outMin, outMax) : outValue;
        }

        public static double GetDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public static void Fill<T>(int x, int y, T targetValue, Func<int, int, T> getter, Action<int, int, T> setter) where T : struct
        {
            var originalValue = getter(x, y);
            var pointsToFill = new Queue<(int, int)>();
            pointsToFill.Enqueue((x, y));

            while (pointsToFill.Count > 0)
            {
                (int px, int py) = pointsToFill.Dequeue();

                if (!targetValue.Equals(getter(px, py)))
                {
                    setter(px, py, targetValue);
                    foreach (var dir in FillDirections)
                    {
                        (int dx, int dy) = dir.GetDeltas();
                        if (originalValue.Equals(getter(px + dx, py + dy)))
                        {
                            pointsToFill.Enqueue((px + dx, py + dy));
                        }
                    }
                }
            }
        }

        private static readonly Direction[] FillDirections = { Direction.Up, Direction.Right, Direction.Down, Direction.Left };
    }
}
