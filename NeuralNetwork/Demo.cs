using BenchmarkDotNet.Attributes;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace NeuralNetwork
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

        // 3 times slower
        [Benchmark(Baseline = true)]
        public void IndexCopy()
        {
            for (int i = 0; i < 100; i++)
            {
                output = weights[0..];
            }
        }

        [Benchmark]
        public void CopyTo()
        {
            for (int i = 0; i < 100; i++)
            {
                weights.CopyTo(output, 0);
            }
        }
    }
}
