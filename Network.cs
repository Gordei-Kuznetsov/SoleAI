using System;

namespace SoleAI
{
    public class Network
    {
        public Network(int numOfInputs, LayerDenseStruct[] layersToAdd)
        {
            Layers = new LayerDense[layersToAdd.Length];

            Layers[0] = new LayerDense((layersToAdd[0].size, numOfInputs), layersToAdd[0].activation);

            for (int i = 1; i < layersToAdd.Length; i++)
            {
                Layers[i] = new LayerDense((layersToAdd[i].size, layersToAdd[i - 1].size), layersToAdd[i].activation);
            }
        }

        private readonly LayerDense[] Layers;

        public void Train(float[,] inputData, int[] expectedOutputs, int batchSize, int epochs)
        {
            int numOfBatches = inputData.GetLength(0);
            if (numOfBatches != expectedOutputs.Length)
            {
                throw new ArgumentException("Number of the provided expected outputs does not match the number of inputs sets.");
            }

            int inputSize = inputData.GetLength(1);
            if (Layers[0].shape.Item2 != inputSize)
            {
                throw new ArgumentException("Number of input values int the inputs does not match the number of weights in the first layer.");
            }

            Console.WriteLine($"Training started.\nBatch size: {batchSize}; Epochs: {epochs}.\n");
            for(int e = 0; e < epochs; e++)
            {
                DateTime start = DateTime.Now;
                Console.WriteLine($"Epoch #{e} started.");

                for (int b = 0; b <= numOfBatches - batchSize; b += batchSize)
                {
                    float[,] inputs = new float[batchSize, inputSize];
                    Array.Copy(inputData, b, inputs, 0, batchSize);

                    for (int l = 0; l < Layers.Length; l++)
                    {
                        inputs = Layers[l].Forward(inputs, batchSize);
                    }

                    int[] correctOutput = new int[batchSize];
                    Array.Copy(expectedOutputs, b, correctOutput, 0, batchSize);

                    float loss = Loss(inputs, correctOutput);

                    string consoleOutput = $"\tBatch completed: Average loss: {loss}";
                    Console.WriteLine(consoleOutput);

                    //BackPropogate()
                    //Maybe some logging
                }

                TimeSpan duration = DateTime.Now - start;
                Console.WriteLine($"Epoch finished in {duration.TotalMilliseconds} ms.\n");
            }
            Console.WriteLine("Training finished.");
        }

        private float Loss(float[,] predictions, int[] correctClasses)
        {
            float negLogProbabilities = 0;
            for (int i = 0; i < correctClasses.Length; i++)
            {
                float targetClass = predictions[i, correctClasses[i]];
                
                // clipping the values to between almost 0 and almost 1 to avoid inf results
                if (targetClass < 1e-7f) { targetClass = 1e-7f; }
                else if (targetClass > 1 - 1e-7f) { targetClass = 1 - 1e-7f; }

                negLogProbabilities += -1 * (float)Math.Log(targetClass);
            }
            return negLogProbabilities / correctClasses.Length;
        }

        public static float[,] Normalize(float[,] values, (int, int) shape, float max, float min)
        {
            if(min >= max)
            {
                throw new ArgumentException("min is greater than or equal to max.");
            }

            float divider = 2 / (max - min);

            for (int a = 0; a < shape.Item1; a++)
            {
                for(int b = 0; b < shape.Item2; b++)
                {
                    values[a,b] = (values[a, b] - min) / divider - 1;
                }
            }

            return values;
        }
    }
}
