using System;

namespace NNLibrary.Activations
{
    public class Sigmoid : IActivation
    {
        public void Process(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    output[b][n] = 1 / (1 + (float)Math.Pow(Math.E, -output[b][n]));
                }
            }
        }
    }
}
