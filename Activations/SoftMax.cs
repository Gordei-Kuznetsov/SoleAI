using System;

namespace SoleAI.Activations
{
    public class SoftMax : IActivation
    {
        public void Act(ref float[][] outputs)
        {
            for (int b = 0; b < outputs.Length; b++)
            {
                // getting max output for outputs

                float max = 0f;
                for (int n = 0; n < outputs[0].Length; n++)
                {
                    if (outputs[b][n] > max) { max = outputs[b][n]; }
                }

                // getting exponent of the outputs and preparing the normalization base

                float normBase = 0;
                for (int n = 0; n < outputs[0].Length; n++)
                {
                    // substracting the max value to reduce the output value in case it is huge,
                    // which could cause overflow when gettting the exponential
                    outputs[b][n] = (float)Math.Pow(Math.E, outputs[b][n] - max);
                    normBase += outputs[b][n];
                }

                // normalizing the outputs' exponents

                for (int n = 0; n < outputs[0].Length; n++)
                {
                    outputs[b][n] = outputs[b][n] / normBase;
                }
            }
        }

        public ValueRange GetValueRange()
        {
            return ValueRange.ZeroToOne;
        }
    }
}
