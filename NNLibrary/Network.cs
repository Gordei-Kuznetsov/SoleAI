using NNLibrary.Activations;
using NNLibrary.Losses;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace NNLibrary
{
    public class Network
    {
        public Network(int numOfInputs, LayerDenseStruct[] layersToAdd)
        {
            Layers = new LayerDense[layersToAdd.Length];
            
            // initializing the first Layer outside the loop as it is does not have a preceding layer to get the inputSize value from
            Layers[0] = new LayerDense((layersToAdd[0].size, numOfInputs), layersToAdd[0].activation);

            for (int i = 1; i < layersToAdd.Length; i++)
            {
                Layers[i] = new LayerDense((layersToAdd[i].size, layersToAdd[i - 1].size), layersToAdd[i].activation);
            }
        }

        private Network(LayerDense[] layers)
        {
            for(int i = 1; i < layers.Length; i++)
            {
                if(layers[i].Shape.Item2 != layers[i - 1].Shape.Item1)
                {
                    throw new Exception("Shape of the neighbouring layers do not match up");
                }
            }
            Layers = layers;
        }

        private LayerDense[] Layers { get; }

        public void Train(float[][] inputData, float[][] expectedOutputs, ILoss lossFunc, int batchSize, int epochs)
        {
            int numOfBatches = inputData.Length;
            if (numOfBatches != expectedOutputs.Length)
            {
                throw new ArgumentException("Number of the provided expected outputs does not match the number of inputs sets.");
            }

            int inputSize = inputData[0].Length;
            if (Layers[0].Shape.Item2 != inputSize)
            {
                throw new ArgumentException("Number of input values int the inputs does not match the number of weights in the first layer.");
            }

            Console.WriteLine($"Training started.\nBatch size: {batchSize}; Epochs: {epochs}.\n");

            for (int e = 0; e < epochs; e++)
            {
                Console.WriteLine($"Epoch #{e + 1} started.");
                Stopwatch timer = Stopwatch.StartNew();

                // iterating through the inputs with batch-sized hops to use the iterable as index for getting the right section of inputs from the array
                // and avoiding getting out of range by substracting the a batch from the iterating range
                for (int b = 0; b <= numOfBatches - batchSize; b += batchSize)
                {
                    // getting next batch of input
                    float[][] inputs = inputData[b..(b + batchSize)];

                    for (int l = 0; l < Layers.Length; l++)
                    {
                        // outputs of the processing become the inputs for next batch
                        inputs = Layers[l].Forward(inputs, batchSize);
                    }

                    // getting next batch of expected output
                    float[][] correctOutputs = expectedOutputs[b..(b + batchSize)];

                    // using the inputs array as it stores outputs from the processing (prdictions) of the last (output) layer
                    float loss = lossFunc.Calc(inputs, correctOutputs);

                    Console.WriteLine($"\tBatch completed: Average loss: {loss}");

                    //BackPropogate()
                    //Maybe some logging
                }

                timer.Stop();

                Console.WriteLine($"Epoch finished in { timer.ElapsedMilliseconds } ms.\n");
            }
            Console.WriteLine("Training finished.\n");
        }

        public static float[][] LoadDataFromCSV(string filename)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            string wholeFile = File.ReadAllText(Path.Combine(projectDirectory, filename));

            wholeFile = wholeFile.Replace('\n', '\r');
            string[] lines = wholeFile.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            int numOfRows = lines.Length;

            float[][] values = new float[numOfRows][];

            for (int r = 0; r < numOfRows; r++)
            {
                values[r] = Array.ConvertAll(lines[r].Split(','), s => float.Parse(s));
            }

            return values;
        }

        public void SaveToJSON(string filename)
        {
            NetworkStruct networkStruct = new NetworkStruct() {
                Shapes = new string[Layers.Length][],
                Weights = new float[Layers.Length][][],
                Biases = new float[Layers.Length][],
                Activations = new string[Layers.Length]
            };
            for(int i = 0; i < Layers.Length; i++)
            {
                networkStruct.Shapes[i] = new string[] { Layers[i].Shape.Item1.ToString(), Layers[i].Shape.Item2.ToString() };
                networkStruct.Weights[i] = Layers[i].Weights;
                networkStruct.Biases[i] = Layers[i].Biases;
                networkStruct.Activations[i] = Layers[i].Activation.GetType().ToString();
            }
            string json = JsonSerializer.Serialize(networkStruct, new JsonSerializerOptions { WriteIndented = true });

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            File.WriteAllText(Path.Combine(projectDirectory, filename), json);
        }

        public static Network LoadFromJSON(string filename)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            string wholeFile = File.ReadAllText(Path.Combine(projectDirectory, filename));

            NetworkStruct networkStruct = JsonSerializer.Deserialize<NetworkStruct>(wholeFile);

            LayerDense[] layers = new LayerDense[networkStruct.Shapes.Length];


            for (int i = 0; i < layers.Length; i++)
            {
                LayerDense layer = new LayerDense(
                    shape: (int.Parse(networkStruct.Shapes[i][0]), int.Parse(networkStruct.Shapes[i][1])),
                    weights: networkStruct.Weights[i],
                    biases: networkStruct.Biases[i],
                    activation: (Activation)Activator.CreateInstance(Type.GetType(networkStruct.Activations[i]))
                );
                layers[i] = layer;
            }

            Network network = new Network(layers);

            return network;
        }
    }
}
