using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace NeuralNetwork
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
