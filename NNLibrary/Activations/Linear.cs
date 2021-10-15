﻿using System;

namespace NNLibrary.Activations
{
    public class Linear : IActivation
    {
        public void Act(ref float[][] output) { }

        public ValueRange GetValueRange()
        {
            return ValueRange.All;
        }

        public Type GetClassName()
        {
            return GetType();
        }
    }
}