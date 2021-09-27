using System;

namespace SoleAI
{
    public class Activation
    {
        public static float[,] ReLU(float[,] outputs, (int, int) shape)
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

            return outputs;
        }

        public static float[,] SoftMax(float[,] values, (int, int) shape)
        {
            float[] normBases = new float[shape.Item1];
            Array.Fill(normBases, 0f);
            for (int n = 0; n < shape.Item1; n++)
            {
                float max = 0f;
                for (int w = 0; w < shape.Item2; w++)
                {
                    if(values[n, w] > max) { max = values[n, w]; }
                }

                for (int w = 0; w < shape.Item2; w++)
                {
                    values[n, w] = (float)Math.Pow(Math.E, values[n, w] - max);
                    normBases[n] += values[n, w];
                }
            }

            for (int n = 0; n < shape.Item1; n++)
            {
                for (int w = 0; w < shape.Item2; w++)
                {
                    values[n, w] = values[n, w] / normBases[n];
                }
            }

            return values;
        }
    }
}
