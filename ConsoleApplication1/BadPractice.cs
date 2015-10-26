using System;
using System.Collections.Generic;
using System.Linq;
using CacheEnumerable;

namespace ConsoleApplication1
{
    public static class BadPractice
    {
        public static List<T> GetAllByPages<T>(IEnumerable<T> seq, int pageSize = 10)
        {
            pageSize = pageSize == 0 ? 10 : pageSize;
            var result = new List<T>();
            var pageNum = 0;
            while (true)
            {
                var page = GetPage(seq, pageNum, pageSize);
                result.AddRange(page);
                if (page.Count < pageSize)
                    break;
                pageNum++;
            }
            return result;
        }

        private static List<T> GetPage<T>(IEnumerable<T> seq, int pageNumber = 0, int pageSize = 10)
        {
            pageSize = pageSize == 0 ? 10 : pageSize;
            return seq.Skip(pageNumber * pageSize).Take(pageSize).ToList();
        }



        public static void PrintOddEven(this IEnumerable<int> seq)
        {
            var arr = seq.AsCacheEnumerable();
            var lst = arr.AsCacheEnumerable();
            arr.Take(10).PrintOdd();
            arr.PrintEven();
        }
        private static void PrintOdd(this IEnumerable<int> seq)
        {
            foreach (var i in seq.Where(i => i % 2 == 1))
                Console.Write(i + ",");
            Console.WriteLine();
        }
        private static void PrintEven(this IEnumerable<int> seq)
        {
            foreach (var i in seq.Where(i => i % 2 == 0))
                Console.Write(i + ",");
            Console.WriteLine();
        }


        public static IEnumerable<int> GetNumbers(string items)
        {
            if (string.IsNullOrEmpty(items))
                return null;

            return items.Split(',').Select(int.Parse);
        }


        public static IEnumerable<IEnumerable<T>> BadPartition<T>(this IEnumerable<T> items, int partitionSize)
        {
            var partition = new List<T>(partitionSize);
            foreach (var item in items)
            {
                partition.Add(item);
                if (partition.Count == partitionSize)
                {
                    yield return partition;
                    partition = new List<T>(partitionSize);
                }
            }
            // Cope with items.Count % partitionSize != 0
            if (partition.Count > 0) yield return partition;
        }
    }
}
