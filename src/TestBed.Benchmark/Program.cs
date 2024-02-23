using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physics;
using Scenarios.Implementations;

namespace TestBed.Benchmark
{
    class Program
    {
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            while (true)
            {
                var world = new World();
                var stack = new BoxPyramidScenario(world);
                var iterations = 400;
                var sw = Stopwatch.StartNew();
                Console.WriteLine("Started");

                for (var i = 0; i < iterations; i++)
                {
                    world.Step(1/60f);
                    if (i%20 == 0)
                    {
                        Console.Clear();
                        Console.WriteLine(i);
                    }
                }
                sw.Stop();
                Console.Clear();
                Console.WriteLine("Finished");
                Console.WriteLine("Total time:\t{0}ms", sw.ElapsedMilliseconds);
                Console.WriteLine("Avg time:\t{0}ms", sw.ElapsedMilliseconds/(float) iterations);

                Console.ReadKey();
            }
        }

        static void TestSinCos()
        {
            var count = 100000000;
            var items = new float[count];
            Console.WriteLine("Generating " + count + " test value..-");

            for (var i = 0; i < count; i++)
            {
                items[i] = rnd.NextFloat(-325, 324);
            }
            Console.WriteLine("Testing...");
            var sw1 = Stopwatch.StartNew();
            for (var i = 0; i < count; i++)
            {
                var x = items[i];
                var q = Math.Sin(x);
            }
            sw1.Stop();
            Console.WriteLine(sw1.ElapsedMilliseconds);

            sw1.Restart();
            for (var i = 0; i < count; i++)
            {
                var x = items[i];
                var q = Math.Cos(x);
            }
            sw1.Stop();
            Console.WriteLine(sw1.ElapsedMilliseconds);

            sw1.Restart();
            for (var i = 0; i < count; i++)
            {
                var x = items[i];
                var q = TrigonoUtil.Sin(x);
            }
            sw1.Stop();
            Console.WriteLine(sw1.ElapsedMilliseconds);

            sw1.Restart();
            for (var i = 0; i < count; i++)
            {
                var x = items[i];
                var q = TrigonoUtil.Cos(x);
            }
            sw1.Stop();
            Console.WriteLine(sw1.ElapsedMilliseconds);

            sw1.Restart();
            for (var i = 0; i < count; i++)
            {
                var x = items[i];
                var q = TrigonoUtil.Sin2(x);
            }
            sw1.Stop();
            Console.WriteLine(sw1.ElapsedMilliseconds);

            Console.ReadKey();
        }
    }
}
