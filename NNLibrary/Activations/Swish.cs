using System;

namespace NNLibrary.Activations
{
    public class Swish : IActivation
    {
        public void Act(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    if (output[b][n] <= 0)
                    {
                        output[b][n] = output[b][n] / (1 + (float)Math.Pow(Math.E, -output[b][n]));
                    }
                }
            }
        }

        public ValueRange GetValueRange()
        {
            return ValueRange.ZeroToInf;
        }

        public Type GetClassName()
        {
            return GetType();
        }
    }
}
