using SoleAI.Activations;
using SoleAI.Losses;
using System;
using System.IO;
using System.Text.Json;

namespace SoleAI
{
    public class Network
    {
        public Network(int numOfInputs, LayerDenseStruct[] layersToAdd)
        {
            _layers = new LayerDense[layersToAdd.Length];
            
            // initializing the first Layer outside the loop as it is does not have a preceding layer to get the inputSize value from
            _layers[0] = new LayerDense((layersToAdd[0].size, numOfInputs), layersToAdd[0].activation);

            for (int i = 1; i < layersToAdd.Length; i++)
            {
                _layers[i] = new LayerDense((layersToAdd[i].size, layersToAdd[i - 1].size), layersToAdd[i].activation);
            }
        }

        public Network(LayerDense[] layers)
        {
            _layers = layers;
        }

        public Network() { }

        private readonly LayerDense[] _layers;

        public LayerDense[] Layers
        {
            get { return _layers; }
        }

        public void Train(float[][] inputData, float[][] expectedOutputs, ILoss lossFunc, int batchSize, int epochs)
        {
            int numOfBatches = inputData.Length;
            if (numOfBatches != expectedOutputs.Length)
            {
                throw new ArgumentException("Number of the provided expected outputs does not match the number of inputs sets.");
            }

            int inputSize = inputData[0].Length;
            if (_layers[0].shape.Item2 != inputSize)
            {
                throw new ArgumentException("Number of input values int the inputs does not match the number of weights in the first layer.");
            }

            Console.WriteLine($"Training started.\nBatch size: {batchSize}; Epochs: {epochs}.\n");

            for (int e = 0; e < epochs; e++)
            {
                DateTime start = DateTime.Now;
                Console.WriteLine($"Epoch #{e + 1} started.");

                // iterating through the inputs with batch-sized hops to use the iterable as index for getting the right section of inputs from the array
                // and avoiding getting out of range by substracting the a batch from the iterating range
                for (int b = 0; b <= numOfBatches - batchSize; b += batchSize)
                {
                    // getting next batch of input
                    float[][] inputs = inputData[b..(b + batchSize)];

                    for (int l = 0; l < _layers.Length; l++)
                    {
                        // outputs of the processing become the inputs for next batch
                        inputs = _layers[l].Forward(inputs, batchSize);
                    }

                    // getting next batch of expected output
                    float[][] correctOutputs = expectedOutputs[b..(b + batchSize)];

                    // using the inputs array as it stores outputs from the processing (prdictions) of the last (output) layer
                    float loss = lossFunc.Calc(inputs, correctOutputs);

                    Console.WriteLine($"\tBatch completed: Average loss: {loss}");

                    //BackPropogate()
                    //Maybe some logging
                }

                TimeSpan duration = DateTime.Now - start;
                Console.WriteLine($"Epoch finished in {duration.TotalMilliseconds} ms.\n");
            }
            Console.WriteLine("Training finished.\n");
        }

        public static float[][] LoadCsv(string filename)
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

        public void SaveToJson(string filename)
        {
            NetworkStruct networkStruct = new NetworkStruct() {
                shapes = new string[Layers.Length][],
                weights = new float[Layers.Length][][],
                biases = new float[Layers.Length][],
                activations = new string[Layers.Length]
            };
            for(int i = 0; i < Layers.Length; i++)
            {
                networkStruct.shapes[i] = new string[] { Layers[i].shape.Item1.ToString(), Layers[i].shape.Item2.ToString() };
                networkStruct.weights[i] = Layers[i].weights;
                networkStruct.biases[i] = Layers[i].biases;
                networkStruct.activations[i] = Layers[i].activation.GetClassName().ToString();
            }
            string json = JsonSerializer.Serialize(networkStruct, new JsonSerializerOptions { WriteIndented = true });

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            File.WriteAllText(Path.Combine(projectDirectory, filename), json);
        }

        public static Network LoadFromJson(string filename)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            string wholeFile = File.ReadAllText(Path.Combine(projectDirectory, filename));

            NetworkStruct networkStruct = JsonSerializer.Deserialize<NetworkStruct>(wholeFile);

            LayerDense[] layers = new LayerDense[networkStruct.shapes.Length];


            for (int i = 0; i < layers.Length; i++)
            {
                LayerDense layer = new LayerDense(
                    shape: (int.Parse(networkStruct.shapes[i][0]), int.Parse(networkStruct.shapes[i][1])),
                    weights: networkStruct.weights[i],
                    biases: networkStruct.biases[i],
                    activation: (IActivation)Activator.CreateInstance(Type.GetType(networkStruct.activations[i]))
                );
                layers[i] = layer;
            }

            Network network = new Network(layers);

            return network;
        }
    }

    public struct NetworkStruct
    {
        public string[][] shapes { get; set; }
        public float[][][] weights { get; set; }
        public float[][] biases { get; set; }
        public string[] activations { get; set; }

    }

    public enum ValueRange
    {
        ZeroToOne,
        NegativeOneToOne,
        ZeroToInf,
        All,
        Sigmoid
    }
}
