using System;

namespace SoleAI
{
    class Program
    {
        static void Main(string[] args)
        {
            float[,] inputs = new float[,]
            {
                { 1f, 2f, 3f, 2.5f },
                { 2.0f, 5.0f, -1.0f, 2.0f },
                { -1.5f, 2.7f, 3.3f, -0.8f },
                { 3.2f, -1.0f, 0.1f, -2.2f },
                {-1.5f, -0.9f, 0.1f, 3.2f }
            };

            (int, int) inputsShape = (5, 4);

            float min = 10f;
            float max = -10f;

            for (int n = 0; n < inputsShape.Item1; n++)
            {
                for (int w = 0; w < inputsShape.Item2; w++)
                {
                    if (inputs[n, w] < min) { min = inputs[n, w]; }
                    if (inputs[n, w] > max) { max = inputs[n, w]; }
                }
            }

            inputs = Network.Normalize(inputs, inputsShape, max, min);

            Network network = new Network(2);

            network.AddLayer((6, 4), Activation.ReLU);
            network.AddLayer((3, 6), Activation.ReLU);

            network.Train(inputs);

            Console.ReadLine();
        }
    }
}
