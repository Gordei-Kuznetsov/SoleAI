using System;

namespace NNLibrary.Activations
{
    public class Softplus : Activation
    {
        public override void Act(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    if (output[b][n] <= 0)
                    {
                        output[b][n] = (float)Math.Log(1 + Math.Pow(Math.E, output[b][n]));
                    }
                }
            }
        }

        public override ValueRange ValueRange { get { return ValueRange.ZeroToInf; } }
    }
}
