using System;

namespace NNLibrary.Activations
{
    public class InverseTanh : Activation
    {
        public override void Act(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    output[b][n] = 0.5f * (float)Math.Log((1 + output[b][n]) / (1 - output[b][n]));
                }
            }
        }

        public override ValueRange ValueRange { get { return ValueRange.NegativeOneToOne; } }
    }
}
