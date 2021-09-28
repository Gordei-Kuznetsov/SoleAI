using BenchmarkDotNet.Attributes;
using System.Text;
using System;
using System.Threading.Tasks;

namespace SoleAI
{
    //[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
    //[SimpleJob(RuntimeMoniker.Net50)]
    //Allows to compare performance on different versions of frameworks.
    //Requires the versions to be installed
    //Modify the project in: <TargetFrameworks>net5.0;net472</TargetFrameworks>

    //[MemoryDiagnoser]
    public class Demo
    {
        public Demo()
        {
            for (int c = 0; c < 20; c++)
            {
                weights[c] = new float[20];
                input[c] = new float[20];
                output[c] = new float[20];
                biases[c] = c;
                for (int r = 0; r < 20; r++)
                {
                    weights[c][r] = 1.23f;
                    input[c][r] = 1.23f;
                }
            }

        }

        float[][] weights = new float[20][];
        float[][] input = new float[20][];
        float[][] output = new float[20][];
        float[] biases = new float[20];


        [Benchmark(Baseline = true)]
        public void ForLoops()
        {
            float min = 0.0f;
            float max = 1.0f;
            for (int a = 0; a < 20; a++)
            {
                for (int b = 0; b < 20; b++)
                {
                    if (weights[a][b] < min) { min = weights[a][b]; }
                    if (weights[a][b] > max) { max = weights[a][b]; }
                }
            }
        }

        // 1.6 times slower
        [Benchmark]
        public void ArrayForeach()
        {
            float min = 0.0f;
            float max = 1.0f;
            Array.ForEach(weights, a => {
                Array.ForEach(a, b => {
                    if (b < min) { min = b; }
                    if (b > max) { max = b; }
                });
            });
        }
    }
}
