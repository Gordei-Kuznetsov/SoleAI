using NNLibrary.Activations;
using System;

namespace NNLibrary
{
    public struct LayerDenseStruct
    {
        public LayerDenseStruct(int size, Activation activation)
        {
            this.size = size;
            this.activation = activation;
        }
        public int size;
        public Activation activation;
    }
}
