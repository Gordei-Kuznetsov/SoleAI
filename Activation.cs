using System;
using System.Collections.Generic;
using System.Text;

namespace SoleAI
{
    public class Activation
    {
        public static float[,] ReLU(float[,] outputs, (int, int) shape)
        {
            for (int a = 0; a < shape.Item1; a++)
            {
                for (int b = 0; b < shape.Item2; b++)
                {
                    if(outputs[a, b] < 0)
                    {
                        outputs[a, b] = 0;
                    }
                }
            }

            return outputs;
        }
    }
}
