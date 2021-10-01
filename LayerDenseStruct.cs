using SoleAI.Activations;
using System;

namespace SoleAI
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
