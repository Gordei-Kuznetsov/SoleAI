using BenchmarkDotNet.Attributes;
using System.Text;

namespace SoleAI
{

    //in the main method:
    //var result = BenchmarkRunner.Run<Demo>();



    //[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
    //[SimpleJob(RuntimeMoniker.Net50)]
    //Allows to compare performance on different versions of frameworks.
    //Requires the versions to be installed
    //Modify the project in: <TargetFrameworks>net5.0;net472</TargetFrameworks>

    [MemoryDiagnoser]
   //Tracks the memory use for the benchmarks
    public class Demo
    {
        [Benchmark(Baseline = true)]
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

        [Benchmark]
        public string GetFullStrWithStrBuilder()
        {
            StringBuilder output = new StringBuilder();
            for(int i = 0; i < 100; i++)
            {
                output.Append(i);
            }
            return output.ToString();
        }
    }
}
