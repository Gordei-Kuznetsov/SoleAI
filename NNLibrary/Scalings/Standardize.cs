using System;

namespace NNLibrary.Scalings
{
    public class Standardize : IScaling
    {
        public Standardize(float[][] values)
        {
            means = new float[values[0].Length];
            divs = new float[values[0].Length];

            for (int i = 0; i < values[0].Length; i++)
            {
                // getting the mean
                float meanSum = 0;
                for (int b = 0; b < values.Length; b++)
                {
                    meanSum += values[b][i];
                }
                float mean = meanSum / values.Length;

                // getting the standard deviation
                float difSum = 0;
                for (int b = 0; b < values.Length; b++)
                {
                    difSum += (float)Math.Pow(values[b][i] - mean, 2);
                }
                float div = (float)Math.Sqrt(difSum / values.Length);

                // saving the values for later to scale the data back up
                means[i] = mean;
                divs[i] = div;

                for (int b = 0; b < values.Length; b++)
                {
                    values[b][i] = (values[b][i] - mean) / div;
                }
            }
        }

        private readonly float[] means;
        private readonly float[] divs;

        public void Denormalize(float[][] values)
        {

        }
    }
}
