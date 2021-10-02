using System;
using SoleAI.Activations;
using SoleAI.Losses;
using SoleAI.Normalizations;

namespace SoleAI
{
    public class App
    {
        public static void Run()
        {
            float[][] inputs = Network.LoadCsv(@"InputData.csv");

            new MinMaxNormalization().Norm(inputs, -1, 1);

            float[][] expectedOutput = Network.LoadCsv(@"OutputData.csv");

            Network network = new Network(inputs[0].Length, new LayerDenseStruct[] {
                new LayerDenseStruct(6, new Tanh()),
                new LayerDenseStruct(6, new Tanh()),
                new LayerDenseStruct(1, new Sigmoid())
            });

            network.Train(inputs, expectedOutput, new MSELoss(), 32, 3);

            Console.ReadLine();
        }
    }
}
