using System;

namespace SoleAI
{
    public class App
    {
        public static void Run()
        {
            float[][] inputs = new float[][]
            {
                new float[] { 1.0f, 2.0f, 3.0f, 2.5f },
                new float[] { 2.0f, 5.0f, -1.0f, 2.0f },
                new float[] { -1.5f, 2.7f, 3.3f, -0.8f },
                new float[] { 3.2f, -1.0f, 0.1f, -2.2f },
                new float[] { -1.5f, -0.9f, 0.1f, 3.2f },
                new float[] { 0.91f, -1.98f, 2.9f, -1.43f }
            };

            Network.MinMaxNormalize(inputs, -1, 1);

            int[] expectedPredictedClasses = new int[] { 1, 0, 0, 2, 1, 2 };

            Network network = new Network(inputs[0].Length, new LayerDenseStruct[] {
                new LayerDenseStruct(6, new ReLU()),
                new LayerDenseStruct(6, new ReLU()),
                new LayerDenseStruct(3, new SoftMax())
            });

            network.Train(inputs, expectedPredictedClasses, 2, 3);

            Console.ReadLine();
        }
    }
}
