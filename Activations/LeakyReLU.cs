using System;

namespace SoleAI.Activations
{
    public class LeakyReLU : IActivation
    {
        public LeakyReLU()
        {
            factor = 0.01f;
        }
        public LeakyReLU(float factor)
        {
            this.factor = factor;
        }

        private readonly float factor;
        public void Act(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    if (output[b][n] < 0)
                    {
                        output[b][n] = factor * output[b][n];
                    }
                }
            }
        }

        public ValueRange GetValueRange()
        {
            return ValueRange.All;
        }

        public Type GetClassName()
        {
            return GetType();
        }
    }
}
