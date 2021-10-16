using System;

namespace NNLibrary.Scalings
{
    public interface IScaling
    {
        void Denormalize(float[][] values);
    }
}
