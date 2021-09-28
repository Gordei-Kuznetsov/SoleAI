using System;

namespace SoleAI
{
    public interface IActivation
    {
        void Forward(float[,] outputs, (int, int) shape);
    }

    public class ReLU : IActivation
    {
        public void Forward(float[,] outputs, (int, int) shape)
        {
            for (int a = 0; a < shape.Item1; a++)
            {
                for (int b = 0; b < shape.Item2; b++)
                {
                    if(outputs[a, b] < 0)
                    {
                        outputs[a, b] = 0;
                    }
                }
            }
        }
    }

    public class SoftMax : IActivation
    {
        public void Forward(float[,] outputs, (int, int) shape)
        {
            for (int b = 0; b < shape.Item1; b++)
            {
                // getting max output for each set of outputs

                float max = 0f;
                for (int n = 0; n < shape.Item2; n++)
                {
                    if (outputs[b, n] > max) { max = outputs[b, n]; }
                }

                // getting exponent of the outputs and preparing the normalization base

                float normBase = 0;
                for (int n = 0; n < shape.Item2; n++)
                {
                    // substracting the max value to reduce the output value in case it is huge,
                    // which could cause overflow when gettting the exponential
                    outputs[b, n] = (float)Math.Pow(Math.E, outputs[b, n] - max);
                    normBase += outputs[b, n];
                }

                // normalizing the exponentiated output

                for (int n = 0; n < shape.Item2; n++)
                {
                    outputs[b, n] = outputs[b, n] / normBase;
                }
            }
        }
    }
}
