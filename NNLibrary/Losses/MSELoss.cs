namespace NNLibrary.Losses
{
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
}