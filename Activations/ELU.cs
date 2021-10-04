using System;

namespace SoleAI.Activations
{
    public class ELU : IActivation
    {
        public ELU(float factor)
        {
            if (factor <= 0)
            {
                throw new ArgumentException("Factor parameter must be greater than zero.");
            }
            this.factor = factor;
        }

        private readonly float factor;
        public void Act(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    if (output[b][n] <= 0)
                    {
                        output[b][n] = factor * ((float)Math.Pow(Math.E, output[b][n]) - 1);
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
