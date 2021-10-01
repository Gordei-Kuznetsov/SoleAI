using SoleAI.Activations;
using System;

namespace SoleAI
{
    public class LayerDense
    {
        public LayerDense((int, int) shape, IActivation activation)
        {
            this.shape = shape;
            this.activation = activation;

            Random rand = new Random();

            weights = new float[shape.Item1][];

            for (int n = 0; n < shape.Item1; n++)
            {
                weights[n] = new float[shape.Item2];
                for (int w = 0; w < shape.Item2; w++)
                {
                    int sign = rand.NextDouble() > 0.5 ? 1 : -1;
                    float weight = (float)rand.NextDouble() * sign;
                    weights[n][w] = weight;
                }
            }

            biases = new float[shape.Item1];
            Array.Fill(biases, 0f);
        }

        public readonly (int, int) shape;
        private readonly float[][] weights;
        private readonly float[] biases;
        private float[][] outputs;
        private readonly IActivation activation;

        public float[][] Forward(float[][] inputBatch, int batchSize)
        {
            Process(inputBatch, batchSize);

            activation.Act(ref outputs);

            return outputs;
        }

        private void Process(float[][] inputBatch, int batchSize)
        {
            outputs = new float[batchSize][];

            for (int b = 0; b < batchSize; b++)
            {
                outputs[b] = new float[shape.Item1];

                for (int n = 0; n < shape.Item1; n++)
                {
                    float dot = 0;

                    for (int w = 0; w < shape.Item2; w++)
                    {
                        dot += weights[n][w] * inputBatch[b][w];
                    }

                    outputs[b][n] = dot + biases[n];
                }
            }
        }
    }
}
