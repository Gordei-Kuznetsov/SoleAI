using NNLibrary.Activations;
using System;

namespace NNLibrary
{
    internal class LayerDense
    {
        public LayerDense((int nodes, int weights) shape, IActivation activation)
        {
            Shape = Validate.LayerDenseShape(shape);
            Activation = activation;

            Random rand = new Random();
            Weights = new float[Shape.nodes][];

            for (int n = 0; n < Shape.nodes; n++)
            {
                Weights[n] = new float[Shape.weights];
                for (int w = 0; w < Shape.weights; w++)
                {
                    int sign = rand.NextDouble() > 0.5 ? 1 : -1;
                    float weight = (float)rand.NextDouble() * sign;
                    Weights[n][w] = weight;
                }
            }

            Biases = new float[Shape.nodes];
            Array.Fill(Biases, 0f);
        }

        public LayerDense((int nodes, int weights) shape, float[][] weights, float[] biases, IActivation activation)
        {
            Shape = Validate.LayerDenseShape(shape);
            Weights = Validate.Weights(weights ,shape);
            Biases = Validate.Biases(biases, shape);
            Activation = activation;
        }

        public (int nodes, int weights) Shape { get; }
        public float[][] Weights { get; }
        public float[] Biases { get; }

        private float[][] Outputs;

        public IActivation Activation { get; }

        public float[][] Forward(float[][] inputBatch, int batchSize)
        {
            Process(inputBatch, batchSize);

            Activation.Process(ref Outputs);

            return Outputs;
        }

        private void Process(float[][] inputBatch, int batchSize)
        {
            Outputs = new float[batchSize][];

            for (int b = 0; b < batchSize; b++)
            {
                Outputs[b] = new float[Shape.nodes];

                for (int n = 0; n < Shape.nodes; n++)
                {
                    float dot = 0;

                    for (int w = 0; w < Shape.weights; w++)
                    {
                        dot += Weights[n][w] * inputBatch[b][w];
                    }

                    Outputs[b][n] = dot + Biases[n];
                }
            }
        }
    }
}
