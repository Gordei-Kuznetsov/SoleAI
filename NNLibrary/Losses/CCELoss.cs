using System;

namespace NNLibrary.Losses
{

    // Categorical Crossentropy (use with softmax)
    public class CCELoss : ILoss
    {
        public float Calc(float[][] predictions, float[][] actualValues)
        {
            float sum = 0;
            for (int a = 0; a < predictions.Length; a++)
            {
                // getting the class/index of the correct class
                int targetClass = Array.FindIndex(actualValues[a], v => v == 1f);

                // the prediction that is supposed to be correct
                float prediction = predictions[a][targetClass];

                // clipping the values to between almost 0 and almost 1 to avoid infinite log result
                if (prediction < 1e-7f) { prediction = 1e-7f; }
                else if (prediction > 1 - 1e-7f) { prediction = 1 - 1e-7f; }

                sum += -1 * (float)Math.Log(prediction);
            }
            // getting mean probability/loss/accuracy
            return sum / predictions.Length;
        }

        public ValueRange GetValueRange()
        {
            return ValueRange.ZeroToOne;
        }
    }
}