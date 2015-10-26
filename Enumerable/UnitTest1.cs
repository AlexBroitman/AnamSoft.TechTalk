using System;
using System.Collections.Generic;
using System.Threading;
using CacheEnumerable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enumerable
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCacheEnumerable()
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

        private static void PrintSeq<T>(IEnumerable<T> seq)
        {
            var start = DateTime.Now;
            foreach (var i in seq)
                Console.Write(i + ",");
            var finish = DateTime.Now;
            Console.WriteLine("\nTook " + (finish - start));
            Console.WriteLine("");
        }
    }
}
