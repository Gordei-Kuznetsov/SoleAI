using System;

namespace SoleAI
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

        private readonly LayerDense[] Layers;

        public void Train(float[][] inputData, int[] expectedOutputs, int batchSize, int epochs)
        {
            int numOfBatches = inputData.Length;
            if (numOfBatches != expectedOutputs.Length)
            {
                throw new ArgumentException("Number of the provided expected outputs does not match the number of inputs sets.");
            }

            int inputSize = inputData[0].Length;
            if (Layers[0].shape.Item2 != inputSize)
            {
                throw new ArgumentException("Number of input values int the inputs does not match the number of weights in the first layer.");
            }

            Console.WriteLine($"Training started.\nBatch size: {batchSize}; Epochs: {epochs}.\n");

            for (int e = 0; e < epochs; e++)
            {
                DateTime start = DateTime.Now;
                Console.WriteLine($"Epoch #{e} started.");

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
                    int[] correctOutputs = expectedOutputs[b..(b + batchSize)];

                    // using the inputs array as it stores outputs from the processing (prdictions) of the last (output) layer
                    float loss = Loss(inputs, correctOutputs);

                    Console.WriteLine($"\tBatch completed: Average loss: {loss}");

                    //BackPropogate()
                    //Maybe some logging
                }

                TimeSpan duration = DateTime.Now - start;
                Console.WriteLine($"Epoch finished in {duration.TotalMilliseconds} ms.\n");
            }
            Console.WriteLine("Training finished.\n");
        }

        private float Loss(float[][] predictions, int[] correctClasses)
        {
            float negLogProbabilities = 0;
            for (int i = 0; i < correctClasses.Length; i++)
            {
                // the prediction that is supposed to be correct
                float targetClass = predictions[i][correctClasses[i]];
                
                // clipping the values to between almost 0 and almost 1 to avoid infinite log result
                if (targetClass < 1e-7f) { targetClass = 1e-7f; }
                else if (targetClass > 1 - 1e-7f) { targetClass = 1 - 1e-7f; }

                negLogProbabilities += -1 * (float)Math.Log(targetClass);
            }
            // getting mean probability/loss/accuracy
            return negLogProbabilities / correctClasses.Length;
        }

        public static void MinMaxNormalize(float[][] values, float lowerBound, float upperBound)
        {
            if(lowerBound >= upperBound)
            {
                throw new ArgumentException("Lower Bound is greater than or equal to Upper Bound.");
            }

            // x' = (u - l) * (x - min) / (max - min) + 1
            // x' = x * ((u - l) / (max - min)) + (min * ((u - l) / (max - min)) + 1)
            // x' = x * K + S

            for (int n = 0; n < values.Length; n++)
            {
                float min = -float.PositiveInfinity;
                float max = float.PositiveInfinity;
                for (int w = 0; w < values[0].Length; w++)
                {
                    if (values[n][w] < min) { min = values[n][w]; }
                    if (values[n][w] > max) { max = values[n][w]; }
                }

                float K = (upperBound - lowerBound) / (max - min);
                float S = min * K + 1;
                for (int b = 0; b < values[0].Length; b++)
                {
                    values[n][b] = K * values[n][b] + S;
                }
            }
        }
    }
}
