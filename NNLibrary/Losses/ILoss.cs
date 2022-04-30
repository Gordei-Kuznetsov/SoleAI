namespace NNLibrary.Losses
{
    public interface ILoss
    {
        float Calc(float[][] predictions, float[][] actualValues);
    }
}