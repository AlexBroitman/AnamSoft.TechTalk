using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CacheEnumerable
{
    internal static class Program
    {
        static void Main()
        {
            Console.WriteLine("Print Fibonacci odd and even");
            FibonacciSequence().PrintOddEven();

            Console.WriteLine("\nPrint Fibonacci odd and even with cache");
            FibonacciSequence().AsCacheEnumerable().PrintOddEven();

            Console.WriteLine("\nTest CacheEnumerable");
            TestCacheEnumerable();
        }

        private static void TestCacheEnumerable()
        {
            var seq = FibonacciSequence();

            // enumerate the sequence twice
            Console.WriteLine("first time wo cache");
            PrintSeq(seq);

            Console.WriteLine("second time wo cache");
            PrintSeq(seq);

            // convert to CacheEnumerable
            seq = seq.AsCacheEnumerable();

            // enumerate twice
            Console.WriteLine("first time with cache");
            PrintSeq(seq);

            Console.WriteLine("second time with cache");
            PrintSeq(seq);

            ((CacheEnumerable<int>)seq).InvalidateCache();
            Console.WriteLine("third time with cache after invalidate");
            PrintSeq(seq);
        }

        private static void PrintSeq<T>(IEnumerable<T> seq)
        {
            var start = DateTime.Now;
            foreach (var i in seq)
                Console.Write(i + ",");
            var finish = DateTime.Now;
            Console.WriteLine("\nTook " + (finish - start));
            Console.WriteLine();
        }

        private static void PrintOddEven(this IEnumerable<int> seq)
        {
            seq.PrintOdd();
            seq.PrintEven();
        }
        private static void PrintOdd(this IEnumerable<int> seq)
        {
            Console.WriteLine("Odd items");
            foreach (var i in seq.Where(i => i % 2 == 1))
                Console.Write(i + ", ");
            Console.WriteLine();
        }

        private static void PrintEven(this IEnumerable<int> seq)
        {
            Console.WriteLine("Even items");
            foreach (var i in seq.Where(i => i % 2 == 0))
                Console.Write(i + ", ");
            Console.WriteLine();
        }

        private static IEnumerable<int> FibonacciSequence()
        {
            yield return 0;
            yield return 1;

            var i1 = 0;
            var i2 = 1;
            while (true)
            {
                Thread.Sleep(100);
                var i3 = i1 + i2;
                if (i3 < i2)
                    break;
                yield return i3;
                i1 = i2;
                i2 = i3;
            }
        }

        private static IEnumerable<int> InfiniteSequence()
        {
            var i = 0;
            while (true)
                yield return i++;
        }
    }
}
