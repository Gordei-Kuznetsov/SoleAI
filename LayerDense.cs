using System;
using System.Collections.Generic;
using System.Text;

namespace SoleAI
{
    public class LayerDense
    {
        public LayerDense((int, int) shape, int batchSize, Func<float[,], (int, int), float[,]> activation)
        {
            this.shape = shape;
            this.batchSize = batchSize;
            this.activation = activation;

            Random rand = new Random();

            float min = 1;
            float max = -1;

            weights = new float[shape.Item1, shape.Item2];

            for (int n = 0; n < shape.Item1; n++)
            {
                for (int w = 0; w < shape.Item2; w++)
                {
                    int sign = rand.Next(0, 1) == 0 ? -1 : 1;
                    float weight = (float)rand.NextDouble() * sign;
                    weights[n, w] = weight;
                    if (weight < min) { min = weight; }
                    if (weight > max) { max = weight; }
                }
            }

            weights = Network.Normalize(weights, shape, max, min);


            biases = new float[shape.Item1];

            for (int i = 0; i < shape.Item1; i++)
            {
                biases[i] = rand.Next(-3, 3);
            }
        }

        public readonly (int, int) shape;
        private readonly int batchSize;
        private float[,] weights;
        private float[] biases;
        private float[,] outputs;
        private readonly Func<float[,], (int, int), float[,]> activation;

        public float[,] Forward(float[,] inputBatch)
        {
            outputs = new float[batchSize, shape.Item1];
            for (int b = 0; b < batchSize; b++)
            {
                for (int n = 0; n < shape.Item1; n++)
                {
                    float dot = 0;
                    for (int w = 0; w < shape.Item2; w++)
                    {
                        dot += weights[n, w] * inputBatch[b, w];
                    }
                    outputs[b, n] = dot + biases[n];
                }
            }

            outputs = activation.Invoke(outputs, (batchSize, shape.Item1));

            return outputs;
        }
    }
}
