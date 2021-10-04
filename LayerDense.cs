using SoleAI.Activations;
using System;

namespace SoleAI
{
    public class LayerDense
    {
        public LayerDense((int, int) shape, IActivation activation)
        {
            _shape = shape;
            _activation = activation;

            Random rand = new Random();

            _weights = new float[shape.Item1][];

            for (int n = 0; n < shape.Item1; n++)
            {
                _weights[n] = new float[shape.Item2];
                for (int w = 0; w < shape.Item2; w++)
                {
                    int sign = rand.NextDouble() > 0.5 ? 1 : -1;
                    float weight = (float)rand.NextDouble() * sign;
                    _weights[n][w] = weight;
                }
            }

            _biases = new float[shape.Item1];
            Array.Fill(_biases, 0f);
        }

        public LayerDense((int, int) shape, float[][] weights, float[] biases, IActivation activation)
        {
            _shape = shape;
            _weights = weights;
            _biases = biases;
            _activation = activation;
        }

        public LayerDense() { }

        private readonly (int, int) _shape;
        public (int, int) shape
        {
            get { return _shape; }
        }

        private readonly float[][] _weights;
        public float[][] weights
        {
            get { return _weights;  }
        }

        private readonly float[] _biases;
        public float[] biases
        {
            get { return _biases; }
        }

        private float[][] outputs;

        private readonly IActivation _activation;
        public IActivation activation
        {
            get { return _activation; }
        }

        public float[][] Forward(float[][] inputBatch, int batchSize)
        {
            Process(inputBatch, batchSize);

            _activation.Act(ref outputs);

            return outputs;
        }

        private void Process(float[][] inputBatch, int batchSize)
        {
            outputs = new float[batchSize][];

            for (int b = 0; b < batchSize; b++)
            {
                outputs[b] = new float[_shape.Item1];

                for (int n = 0; n < _shape.Item1; n++)
                {
                    float dot = 0;

                    for (int w = 0; w < _shape.Item2; w++)
                    {
                        dot += _weights[n][w] * inputBatch[b][w];
                    }

                    outputs[b][n] = dot + _biases[n];
                }
            }
        }
    }
}
