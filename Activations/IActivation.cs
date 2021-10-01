﻿namespace SoleAI.Activations
{
    public interface IActivation
    {
        void Act(ref float[][] output);

        ValueRange GetValueRange();
    }
}