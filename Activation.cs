using System;

namespace SoleAI
{
    public class Activation
    {
        // Need to restructure the system around passing the activation function to the layer
        // ReLU can be executed for each output in the DenseLayer.Forward method
        // instead of running another set of nested loops
        // should be a huge time saver as it is going to be used on most of the layers in the nn
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

            // this could be the Process method being called with a parameter for the function to do the ```values[n, w] / normBases[n]```
            // something like:
            // Process(Func<float, float, float> activation)
            // {
            //      ...
            //      values[n,w] = activation.Invoke(values[n,w], normBases[n]);
            //      ...
            // }
            for (int n = 0; n < shape.Item1; n++)
            {
                // this inner loop can go to after the second inner loop in the previous section
                for (int w = 0; w < shape.Item2; w++)
                {
                    values[n, w] = values[n, w] / normBases[n];
                }
            }

            return values;
        }
    }
}
