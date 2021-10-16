using System;

namespace NNLibrary.Activations
{
    public class Algebraic : Activation
    {
        public override void Act(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    output[b][n] = output[b][n] / (float)Math.Sqrt(1 + output[b][n] * output[b][n]);
                }
            }
        }

        public override ValueRange ValueRange { get { return ValueRange.NegativeOneToOne; } }
    }
}