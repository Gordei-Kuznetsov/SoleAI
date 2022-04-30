using NNLibrary.Activations;
using System;

namespace NNLibrary
{
    public struct LayerDenseStruct
    {
        public LayerDenseStruct(int size, Activation activation)
        {
            Size = size;
            Activation = activation;
        }
        public int Size { get; set; }
        public Activation Activation { get; set; }
    }
}
