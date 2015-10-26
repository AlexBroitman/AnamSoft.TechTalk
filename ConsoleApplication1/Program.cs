using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CacheEnumerable;

namespace ConsoleApplication1
{
    internal sealed class Program
    {
        static void Main()
        {
            FibonacciSequence().PrintOddEven();
            //TestCacheEnumerable();
            //TestPartition(Enumerable.Range(0,999));
            //TestGetNumbers();
        }

        private static void TestGetNumbers()
        {
            const string s1 = "1,2,3,4,5,6,7,8,9,10";
            const string s2 = "1,2,3,4,5,6,7,8,9,10,";

            foreach (var i in BadPractice.GetNumbers(s1))
                Console.WriteLine(i);
            Console.WriteLine();
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
            Console.WriteLine("first time cache with cache");
            PrintSeq(seq);

            Console.WriteLine("second time cache with cache");
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


        private static void TestPartition(IEnumerable<int> seq, int partitionSize = 10)
        {
            foreach (var group in seq.BadPartition(partitionSize).ToArray())
            {
                foreach (var i in group)
                    Console.Write(i + ",");
                Console.WriteLine();
            }
        }


        private static IEnumerable<int> FibonacciSequence()
        {
            yield return 0;
            yield return 1;

            var i1 = 0;
            var i2 = 1;
            while (true)
            {
                Thread.Sleep(400);
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
