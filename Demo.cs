using BenchmarkDotNet.Attributes;
using System.Text;
using System;
using System.Threading.Tasks;

namespace SoleAI
{

    //in the main method:
    //var result = BenchmarkRunner.Run<Demo>();



    //[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
    //[SimpleJob(RuntimeMoniker.Net50)]
    //Allows to compare performance on different versions of frameworks.
    //Requires the versions to be installed
    //Modify the project in: <TargetFrameworks>net5.0;net472</TargetFrameworks>


    //Tracks the memory use for the benchmarks
    //[MemoryDiagnoser]
    public class Demo
    {
        //[Benchmark(Baseline = true)]
        //Attributes that marks the method as the target for the benchmarking
        //Baseline tells to compare the benchmarks of other methods to this one
        //and produce ratio
        public string GetFullStringNormally()
        {
            string output = "";
            for(int i = 0; i < 100; i++)
            {
                output += i;
            }
            return output;
        }

        //[Benchmark]
        public string GetFullStrWithStrBuilder()
        {
            StringBuilder output = new StringBuilder();
            for(int i = 0; i < 100; i++)
            {
                output.Append(i);
            }
            return output.ToString();
        }

        public Demo()
        {
            for (int c = 0; c < 20; c++)
            {
                //arrayOfArrays[c] = new float[20];
                biases[c] = c;
                for (int r = 0; r < 20; r++)
                {
                    input[c, r] = 1.23f;
                    multiArray[c, r] = 1.23f;
                    //arrayOfArrays[c][r] = 1.23f;
                }
            }

        }

        float[,] multiArray = new float[20, 20];
        float[][] arrayOfArrays = new float[20][];
        float[,] input = new float[20, 20];
        float[,] output = new float[20, 20];
        float[] biases = new float[20];

        //[Benchmark]
        public void AssignMultidimentionalArray()
        {
            for(int b = 0; b < 20; b++)
            {
                for (int c = 0; c < 20; c++)
                {
                    for (int r = 0; r < 20; r++)
                    {
                        multiArray[c, r] = multiArray[c, r] * input[b, r];
                    }
                }
            }
        }

        //[Benchmark(Baseline = true)]
        public void AssignArrayOfArrays()
        {
            for (int b = 0; b < 20; b++)
            {
                for (int c = 0; c < 20; c++)
                {
                    for (int r = 0; r < 20; r++)
                    {
                        arrayOfArrays[c][r] = arrayOfArrays[c][r] * input[b, r];
                    }
                }
            }
        }

        [Benchmark(Baseline = true)]
        public void NormalDot()
        {
            for (int b = 0; b < 20; b++)
            {
                for (int c = 0; c < 20; c++)
                {
                    float dot = 0;
                    for (int r = 0; r < 20; r++)
                    {
                        dot += multiArray[c, r] * input[b, r];
                    }
                    output[b, c] = dot + biases[c];
                }
            }
        }

        [Benchmark]
        public void ParallelDot()
        {
            Parallel.For(0, 20, b =>
            {
                for (int c = 0; c < 20; c++)
                {
                    float dot = 0;
                    for (int r = 0; r < 20; r++)
                    {
                        dot += multiArray[c, r] * input[b, r];
                    }
                    output[b, c] = dot + biases[c];
                }
            });
        }
    }
}
