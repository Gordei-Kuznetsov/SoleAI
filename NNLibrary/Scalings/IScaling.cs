using System;

namespace NNLibrary.Scalings
{
    interface IScaling
    {
        void Denormalize(float[][] values);
    }
}
