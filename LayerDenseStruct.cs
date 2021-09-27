using System;

namespace SoleAI
{
    public struct LayerDenseStruct
    {
        public LayerDenseStruct(int size, Func<float[,], (int, int), float[,]> activation)
        {
            this.size = size;
            this.activation = activation;
        }
        public int size;
        public Func<float[,], (int, int), float[,]> activation;
    }
}
