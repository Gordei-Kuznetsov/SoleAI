using System;

namespace NNLibrary.Activations
{
    public class Linear : Activation
    {
        public override void Act(ref float[][] output) { }

        public override ValueRange ValueRange { get { return ValueRange.All; } }
    }
}
