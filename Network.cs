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

            // declaring the arrays for reuse in the nested loop
            float[,] inputs = new float[batchSize, inputSize];
            int[] correctOutput = new int[batchSize];

            for (int e = 0; e < epochs; e++)
            {
                DateTime start = DateTime.Now;
                Console.WriteLine($"Epoch #{e} started.");

                // iterating through the inputs with batch-sized hops to use the iterable as index for getting the right section of inputs from the array
                // and avoiding getting out of range by substracting the a batch from the iterating range
                for (int b = 0; b <= numOfBatches - batchSize; b += batchSize)
                {
                    // getting next batch of input
                    Copy(inputData, b, inputs, batchSize);

                    for (int l = 0; l < Layers.Length; l++)
                    {
                        // outputs of the processing become the inputs for next batch
                        inputs = Layers[l].Forward(inputs, batchSize);
                    }
                    
                    // getting next batch of expected output
                    Array.Copy(expectedOutputs, b, correctOutput, 0, batchSize);

                    // using the inputs array as it stores outputs from the processing (prdictions) of the last (output) layer
                    float loss = Loss(inputs, correctOutput);

                    Console.WriteLine($"\tBatch completed: Average loss: {loss}");

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
                // the prediction that is supposed to be correct
                float targetClass = predictions[i, correctClasses[i]];
                
                // clipping the values to between almost 0 and almost 1 to avoid infinite log result
                if (targetClass < 1e-7f) { targetClass = 1e-7f; }
                else if (targetClass > 1 - 1e-7f) { targetClass = 1 - 1e-7f; }

                negLogProbabilities += -1 * (float)Math.Log(targetClass);
            }
            // getting mean probability/loss/accuracy
            return negLogProbabilities / correctClasses.Length;
        }

        private void Copy(float[,] source, int sourceIndx, float[,] dest, int len)
        {
            int sourceShape = source.GetLength(1);
            for (int a = sourceIndx; a < sourceIndx + len; a++)
            {
                for (int b = 0; b < sourceShape; b++)
                {
                    dest[a - sourceIndx, b] = source[a, b];
                }
            }
        }

        public static float[,] Normalize(float[,] values, (int, int) shape, float max, float min)
        {
            if(min >= max)
            {
                throw new ArgumentException("min is greater than or equal to max.");
            }

            // x" = 2 * (x - min) / (max - min) - 1
            // x" = x * (2 / (max - min)) - (min * (2 / (max - min)) + 1)
            float K = 2 / (max - min);
            float S = min * K + 1;

            for (int a = 0; a < shape.Item1; a++)
            {
                for(int b = 0; b < shape.Item2; b++)
                {
                    values[a, b] = K * values[a, b] - S;
                }
            }

            return values;
        }
    }
}
