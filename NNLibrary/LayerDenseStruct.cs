using NNLibrary.Activations;
using System;

namespace NNLibrary
{
    public struct LayerDenseStruct
    {
        public LayerDenseStruct(int size, IActivation activation)
        {
            this.size = size;
            this.activation = activation;
        }
        public int size;
        public IActivation activation;
    }
}
