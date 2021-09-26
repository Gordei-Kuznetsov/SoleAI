using System;
using System.Collections.Generic;

namespace SoleAI
{
    public class Network
    {
        public Network(int batchSize)
        {
            Layers = new List<LayerDense>();
            this.batchSize = batchSize;
        }

        private readonly List<LayerDense> Layers;
        private readonly int batchSize;

        public void AddLayer((int, int) shape, Func<float[,], (int, int), float[,]> activation)
        {
            if(Layers.Count > 0 && shape.Item2 != Layers[Layers.Count - 1].shape.Item1)
            {
                throw new ArgumentException("Number of inputs for the layer to be added does not match the size of the last layer.");
            }

            Layers.Add(new LayerDense(shape, batchSize, activation));
        }

        public void Train(float[,] inputs)
        {
            int lengthsOfBatches = inputs.GetLength(1);
            if (Layers[0].shape.Item2 != lengthsOfBatches)
            {
                throw new ArgumentException("Number of input values int the inputs does not match the number of weights in the first layer.");
            }

            int numOfBatches = inputs.GetLength(0);
            for(int i = 0; i <= numOfBatches - batchSize; i += batchSize)
            {
                float[,] nextBatch = new float[batchSize, lengthsOfBatches];
                Array.Copy(inputs, i, nextBatch, 0, batchSize);

                for (int l = 0; l < Layers.Count; l++)
                {
                    nextBatch = Layers[l].Forward(nextBatch);
                }

                //BackPropogate()
            }
        }

        public static float[,] Normalize(float[,] values, (int, int) shape, float max, float min)
        {
            if(min >= max)
            {
                throw new ArgumentException("min is greater than or equal to max.");
            }

            float divider = (max - min) / 2;

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
