using NNLibrary.Activations;
using System;

namespace NNLibrary
{
    public class LayerDense
    {
        public LayerDense((int, int) shape, IActivation activation)
        {
            Shape = shape;
            Activation = activation;

            Random rand = new Random();

            Weights = new float[shape.Item1][];

            for (int n = 0; n < shape.Item1; n++)
            {
                Weights[n] = new float[shape.Item2];
                for (int w = 0; w < shape.Item2; w++)
                {
                    int sign = rand.NextDouble() > 0.5 ? 1 : -1;
                    float weight = (float)rand.NextDouble() * sign;
                    Weights[n][w] = weight;
                }
            }

            Biases = new float[shape.Item1];
            Array.Fill(Biases, 0f);
        }

        public LayerDense((int, int) shape, float[][] weights, float[] biases, IActivation activation)
        {
            if (weights.Length != shape.Item1 ||
                weights[0].Length != shape.Item2 ||
                biases.Length != shape.Item1)
            {
                throw new ArgumentException("The layer's shape is inconsistent with the shapes of other parameters");
            }

            Shape = shape;
            Weights = weights;
            Biases = biases;
            Activation = activation;
        }

        public (int, int) Shape { get; }
        public float[][] Weights { get; }
        public float[] Biases { get; }

        private float[][] outputs;

        public IActivation Activation { get; }

        public float[][] Forward(float[][] inputBatch, int batchSize)
        {
            Process(inputBatch, batchSize);

            Activation.Act(ref outputs);

            return outputs;
        }

        private void Process(float[][] inputBatch, int batchSize)
        {
            outputs = new float[batchSize][];

            for (int b = 0; b < batchSize; b++)
            {
                outputs[b] = new float[Shape.Item1];

                for (int n = 0; n < Shape.Item1; n++)
                {
                    float dot = 0;

                    for (int w = 0; w < Shape.Item2; w++)
                    {
                        dot += Weights[n][w] * inputBatch[b][w];
                    }

                    outputs[b][n] = dot + Biases[n];
                }
            }
        }
    }
}
