using System;

namespace NNLibrary.Activations
{
    public interface IActivation
    {
        void Process(ref float[][] output);
    }
}
