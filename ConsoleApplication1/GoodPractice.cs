using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1
{
    static class GoodPractice
    {
        public static IEnumerable<int> GetNumbers(string items)
        {
            if (string.IsNullOrEmpty(items))
                return Enumerable.Empty<int>();

            var i = 0;
            return items.Split(',')
                .Where(s => int.TryParse(s, out i))
                .Select(s => i);
        }


        public static IEnumerable<IEnumerable<T>> GoodPartition<T>(this IEnumerable<T> items, int partitionSize)
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
