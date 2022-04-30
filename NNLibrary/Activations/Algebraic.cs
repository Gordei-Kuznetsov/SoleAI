using System;

namespace NNLibrary.Activations
{
    public class Algebraic : IActivation
    {
        public void Process(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    output[b][n] = output[b][n] / (float)Math.Sqrt(1 + output[b][n] * output[b][n]);
                }
            }
        }
    }
}