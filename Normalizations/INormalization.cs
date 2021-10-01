using System;

namespace SoleAI.Normalizations
{
    interface INormalization
    {
        void Norm(float[][] values, float lowerBound, float upperBound);
    }
}
