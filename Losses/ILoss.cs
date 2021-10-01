namespace SoleAI.Losses
{
    public interface ILoss
    {
        float Calc(float[][] predictions, float[][] actualValues);
        ValueRange GetValueRange();
    }
}