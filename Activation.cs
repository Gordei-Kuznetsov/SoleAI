using System;

namespace SoleAI
{
    public interface IActivation
    {
        void Act(ref float[][] output);
    }

    public class Linear : IActivation
    {
        public void Act(ref float[][] output) { }
    }

    public class ReLU : IActivation
    {
        public void Act(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    if (output[b][n] < 0)
                    {
                        output[b][n] = 0;
                    }
                }
            }
        }
    }

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
    }

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
    }

    public class Softplus : IActivation
    {
        public void Act(ref float[][] output)
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
    }

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
    }

    public class Tanh : IActivation
    {
        public void Act(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    output[b][n] = (float)Math.Tanh(output[b][n]);
                }
            }
        }
    }

    public class InverseTanh : IActivation
    {
        public void Act(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    output[b][n] = 0.5f * (float)Math.Log((1 + output[b][n]) / (1 - output[b][n]));
                }
            }
        }
    }

    public class Logistic : IActivation
    {
        public void Act(ref float[][] output)
        {
            for (int b = 0; b < output.Length; b++)
            {
                for (int n = 0; n < output[0].Length; n++)
                {
                    output[b][n] = output[b][n] / (1 + (float)Math.Pow(Math.E, output[b][n]));
                }
            }
        }
    }

    public class Algebraic : IActivation
    {
        public void Act(ref float[][] output)
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

    public class Sigmoid : IActivation
    {
        public void Act(ref float[][] output)
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

    public class SoftMax : IActivation
    {
        public void Act(ref float[][] outputs)
        {
            for(int b = 0; b < outputs.Length; b++)
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
    }
}
