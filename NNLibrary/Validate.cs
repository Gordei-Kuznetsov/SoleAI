using System;
using System.Collections.Generic;
using System.Text;

namespace NNLibrary
{
    internal static class Validate
    {
        internal static (int, int) LayerDenseShape((int nodes, int weights) shape)
        {
            if (shape.nodes <= 0 || shape.weights <= 0)
            {
                throw new ArgumentException($"The layer shape ({ shape.nodes }, { shape.weights }) is not valid.");
            }
            return shape;
        }

        internal static float[][] Weights(float[][] weights, (int nodes, int weights) shape)
        {
            if (weights == null)
            {
                throw new ArgumentException("The are no weights passed in.");
            }
            if (weights.Length != shape.nodes || weights[0].Length != shape.weights)
            {
                throw new ArgumentException("The layer's weights are inconsistent with the layer's shape.");
            }
            return weights;
        }

        internal static float[] Biases(float[] biases, (int nodes, int weights) shape)
        {
            if (biases == null)
            {
                throw new ArgumentException("The are no biases passed in.");
            }
            if (biases.Length != shape.nodes)
            {
                throw new ArgumentException("The layer's biases are inconsistent with the layer's shape.");
            }
            return biases;
        }

        internal static LayerDenseStruct[] LayerDenseStructs(LayerDenseStruct[] layers)
        {
            foreach(LayerDenseStruct layer in layers)
            {
                if (layer.Size <= 0)
                {
                    throw new ArgumentException("The layer's size is not valid.");
                }
                if (layer.Activation == null)
                {
                    throw new ArgumentException("No activation provided for the layer.");
                }
            }
            return layers;
        }

        internal static LayerDense[] DenseLayers(LayerDense[] layers)
        {
            for (int i = 1; i < layers.Length; i++)
            {
                if (layers[i].Shape.weights != layers[i - 1].Shape.nodes)
                {
                    throw new ArgumentException("Shape of the neighbouring layers do not match up");
                }
            }
            return layers;
        }

        internal static void TrainingData(float[][] inputData, float[][] expectedOutputs, int inputSize, int outputSize)
        {
            if (inputData.Length != expectedOutputs.Length)
            {
                throw new ArgumentException("Number of the provided expected output sets does not match the number of input sets.");
            }
            if (inputData[0].Length != inputSize)
            {
                throw new ArgumentException("Number of values in the inputs does not match the number of weights in the first layer.");
            }
            if (expectedOutputs[0].Length != outputSize)
            {
                throw new ArgumentException("Number of values in the expected outputs does not match the number of nodes in the last layer.");
            }
        }
    }
}
