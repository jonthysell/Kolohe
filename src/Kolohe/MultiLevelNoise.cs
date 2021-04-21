﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Kolohe
{
    public class MultiLevelNoise
    {
        private readonly OpenSimplex2F[] _noise;

        public MultiLevelNoise(long[] seeds)
        {
            _noise = new OpenSimplex2F[seeds.Length];
            for (int i = 0; i < seeds.Length; i++)
            {
                _noise[i] = new OpenSimplex2F(seeds[i]);
            }
        }

        public double Noise2(double x, double y)
        {
            double result = 0;
            double frequency = 1.0;
            double amplitude = 1.0;

            for (int i = 0; i < _noise.Length; i++)
            {
                result += _noise[i].Noise2(x * frequency, y * frequency) * amplitude;
                frequency *= 2;
                amplitude *= 0.5;
            }

            return result;
        }

        public static MultiLevelNoise Generate(Random random, int octaves)
        {
            var seeds = new long[octaves];
            var buffer = new byte[8];
            for (int i = 0; i < octaves; i++)
            {
                random.NextBytes(buffer);
                seeds[i] = BitConverter.ToInt64(buffer, 0);
            }
            return new MultiLevelNoise(seeds);
        }
    }
}
