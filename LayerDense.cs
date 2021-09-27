using System;

namespace SoleAI
{
    public class LayerDense
    {
        public LayerDense((int, int) shape, Func<float[,], (int, int), float[,]> activation)
        {
            this.shape = shape;
            this.activation = activation;

            Random rand = new Random();

            weights = new float[shape.Item1, shape.Item2];

            for (int n = 0; n < shape.Item1; n++)
            {
                for (int w = 0; w < shape.Item2; w++)
                {
                    int sign = rand.Next(-1, 1) == -1 ? -1 : 1;
                    float weight = (float)rand.NextDouble() * sign;
                    weights[n, w] = weight;
                }
            }

            //Don't know why I even did the normilization of already normalized values
            //weights = Network.Normalize(weights, shape, max, min);


            biases = new float[shape.Item1];
            Array.Fill(biases, 0f);
            //for (int i = 0; i < shape.Item1; i++)
            //{
            //    biases[i] = rand.Next(-3, 3);
            //}
        }

        public readonly (int, int) shape;
        private readonly float[,] weights;
        private readonly float[] biases;
        private float[,] outputs;
        private readonly Func<float[,], (int, int), float[,]> activation;

        public float[,] Forward(float[,] inputBatch, int batchSize)
        {
            outputs = new float[batchSize, shape.Item1];

            Process(inputBatch, batchSize);

            outputs = activation.Invoke(outputs, (batchSize, shape.Item1));

            return outputs;
        }

        private void Process(float[,] inputBatch, int batchSize)
        {
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
        }
    }
}
