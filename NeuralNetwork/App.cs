using System;
using System.Threading.Tasks;
using NNLibrary.Activations;
using NNLibrary.Losses;
using NNLibrary.Scalings;
using NNLibrary;

namespace NeuralNetwork
{
    public class App
    {
        public static void Run()
        {
            float[][] inputs = Network.LoadDataFromCSV(@"InputData.csv");

            MinMaxNormalize inputScaler = new MinMaxNormalize(inputs, -1, 1);

            float[][] expectedOutput = Network.LoadDataFromCSV(@"OutputData.csv");

            Network network = new Network(inputs[0].Length, new LayerDenseStruct[] {
                new LayerDenseStruct(6, new Tanh()),
                new LayerDenseStruct(6, new Tanh()),
                new LayerDenseStruct(1, new Sigmoid())
            });

            network.Train(inputs, expectedOutput, new MSELoss(), 32, 3);
            
            network.SaveToJSON("network.json");

            Network newNetwork = Network.LoadFromJSON("network.json");

            Console.WriteLine("\nNew Network Training\n");

            newNetwork.Train(inputs[..64], expectedOutput[..64], new MSELoss(), 32, 3);
            
            Console.ReadLine();
        }
    }
}
