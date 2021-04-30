// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Kolohe
{
    public class MultiLevelNoise
    {
        public readonly long[] Seeds;

        public readonly double Min;

        public readonly double Max;

        public readonly double FrequencyMultiplier;

        public readonly double AmplitudeMultiplier;

        private readonly OpenSimplex2F[] _noise;


        public MultiLevelNoise(long[] seeds, double min = 0, double max = 1.0, double frequencyMultiplier = 1.0, double amplitudeMultiplier = 1.0)
        {
            Seeds = new long[seeds.Length];
            Array.Copy(seeds, Seeds, seeds.Length);

            Min = min;
            Max = min <= max ? max : throw new ArgumentOutOfRangeException(nameof(max));
            FrequencyMultiplier = frequencyMultiplier >= 1.0 ? frequencyMultiplier : throw new ArgumentOutOfRangeException(nameof(frequencyMultiplier));
            AmplitudeMultiplier = amplitudeMultiplier > 0.0 && amplitudeMultiplier <= 1.0 ? amplitudeMultiplier : throw new ArgumentOutOfRangeException(nameof(amplitudeMultiplier));

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
                result += MathExt.RemapValue(_noise[i].Noise2(x / frequency, y / frequency), -1, 1, Min, Max) * amplitude;
                frequency *= FrequencyMultiplier;
                amplitude *= AmplitudeMultiplier;
            }

            return result;
        }

        public static MultiLevelNoise Generate(Random random, int octaves, double min = 0, double max = 1.0, double frequencyMultiplier = 1.0, double amplitudeMultiplier = 1.0)
        {
            var seeds = new long[octaves];
            var buffer = new byte[8];
            for (int i = 0; i < octaves; i++)
            {
                random.NextBytes(buffer);
                seeds[i] = BitConverter.ToInt64(buffer, 0);
            }
            return new MultiLevelNoise(seeds, min, max, frequencyMultiplier, amplitudeMultiplier);
        }
    }
}
