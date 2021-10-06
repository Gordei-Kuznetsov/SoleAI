using System;

namespace SoleAI.Scalings
{
    interface IScaling
    {
        void Denormalize(float[][] values);
    }
}
