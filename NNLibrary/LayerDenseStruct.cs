using NNLibrary.Activations;
using System;

namespace NNLibrary
{
    public struct LayerDenseStruct
    {
        public LayerDenseStruct(int size, IActivation activation)
        {
            Size = size;
            Activation = activation;
        }
        public int Size { get; set; }
        public IActivation Activation { get; set; }
    }
}
