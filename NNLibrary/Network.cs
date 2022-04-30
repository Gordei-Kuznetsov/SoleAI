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
            if (numOfInputs <= 0)
            {
                throw new ArgumentException("The number of inputs is invalid.");
            }
            Validate.LayerDenseStructs(layersToAdd);

            Layers = new LayerDense[layersToAdd.Length];
            Layers[0] = new LayerDense((layersToAdd[0].Size, numOfInputs), layersToAdd[0].Activation);
            for (int i = 1; i < layersToAdd.Length; i++)
            {
                Layers[i] = new LayerDense((layersToAdd[i].Size, layersToAdd[i - 1].Size), layersToAdd[i].Activation);
            }
        }

        private Network(LayerDense[] layers)
        {
            Layers = Validate.DenseLayers(layers);
        }

        private LayerDense[] Layers { get; }

        public void Train(float[][] inputData, float[][] expectedOutputs, ILoss lossFunc, int batchSize, int epochs)
        {
            Validate.TrainingData(inputData, expectedOutputs, Layers[0].Shape.weights, Layers[Layers.Length - 1].Shape.nodes);

            Console.WriteLine($"Training started.\nBatch size: { batchSize }; Epochs: { epochs }.\n");

            int numOfBatches = inputData.Length;
            for (int e = 0; e < epochs; e++)
            {
                Console.WriteLine($"Epoch #{ e + 1 } started.");
                Stopwatch timer = Stopwatch.StartNew();

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

                    Console.WriteLine($"\tBatch completed: Average loss: { loss }");

                    //BackPropogate()
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
                networkStruct.Shapes[i] = new string[] { Layers[i].Shape.nodes.ToString(), Layers[i].Shape.weights.ToString() };
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

            try
            {
                NetworkStruct networkStruct = JsonSerializer.Deserialize<NetworkStruct>(wholeFile);
                LayerDense[]  layers = new LayerDense[networkStruct.Shapes.Length];

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
            catch (Exception ex)
            {
                Console.WriteLine($"Somethng went wrong while loading the saved network.\nError message: { ex.Message }\nStack trace:\n{ ex.StackTrace }");
                return null;
            }
        }
    }
}
