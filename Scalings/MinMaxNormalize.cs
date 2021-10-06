using System;

namespace SoleAI.Scalings
{
    public class MinMaxNormalize : IScaling
    {
        public MinMaxNormalize(float[][] values, float lowerBound, float upperBound)
        {
            if (lowerBound >= upperBound)
            {
                throw new ArgumentException("Lower Bound is greater than or equal to Upper Bound.");
            }

            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
            mins = new float[values[0].Length];
            maxs = new float[values[0].Length];

            // x' = (up - low) * (x - min) / (max - min) + low
            // x' = x * ((up - low) / (max - min)) + (min * ((up - low) / (max - min)) + 1)
            // x' = x * K + S

            for (int i = 0; i < values[0].Length; i++)
            {
                float min = float.PositiveInfinity;
                float max = -float.PositiveInfinity;
                for (int b = 0; b < values.Length; b++)
                {
                    if (values[b][i] < min) { min = values[b][i]; }
                    if (values[b][i] > max) { max = values[b][i]; }
                }

                // saving the values for later to scale the data back up
                mins[i] = min;
                maxs[i] = max;

                float K = (upperBound - lowerBound) / (max - min);
                float S = -min * K + lowerBound;
                for (int b = 0; b < values.Length; b++)
                {
                    values[b][i] = K * values[b][i] + S;
                }
            }
        }

        private readonly float lowerBound;
        private readonly float upperBound;
        private readonly float[] mins;
        private readonly float[] maxs;

        public void Denormalize(float[][] values)
        {
            
        }
    }
}
