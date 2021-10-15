using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace SoleAI
{
    class Program
    {
        static void Main(string[] args)
        {
            App.Run();

            // BenchmarkRunner.Run<Demo>();
        }
    }
}
