using System;

namespace NNLibrary.Activations
{
    public abstract class Activation
    {
        public abstract void Act(ref float[][] output);

        public abstract ValueRange ValueRange { get; }
    }
}
