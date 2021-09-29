using System;
using System.Collections.Generic;
using System.Text;

namespace SoleAI
{
    public interface ILoss
    {
        float Calc(float[][] predictions, float[][] actualValues);
    }

    // Mean Squared Error (use something with result between 0 and 1)
    public class MSELoss : ILoss
    {
        public float Calc(float[][] predictions, float[][] actualValues)
        {
            float sum = 0;

            for(int a = 0; a < predictions.Length; a++)
            {
                float dif = actualValues[a][0] - predictions[a][0];
                sum += dif * dif;
            }

            return sum / predictions.Length;
        }
    }

    // Binary Crossentropy / Log Loss (use with sigmoid)
    public class BCELoss : ILoss
    {
        public float Calc(float[][] predictions, float[][] actualValues)
        {
            return 0;
        }
    }

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
    }
}