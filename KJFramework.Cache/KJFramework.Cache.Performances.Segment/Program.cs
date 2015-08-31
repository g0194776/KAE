using System;
using System.Linq;
using KJFramework.Containers;
using KJFramework.Cores.Segments;
using KJFramework.Timer;

namespace KJFramework.Cache.Performances.Segment
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            Console.WriteLine("#Initialize id & random number......800,000");
            int[] ids = new int[800000];
            int[] randomNumbers = new int[800000];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = random.Next(10, 10000);
                randomNumbers[i] = random.Next(10, 2048);
            }
            long totalSize = randomNumbers.Sum();
            Console.WriteLine("#Initialize segment cache container......");
            ISegmentCachePolicy policy = new SegmentCachePolicy(
                                                                    new SegmentSizePair { Size = 1024, Percent = 0.2F },
                                                                    new SegmentSizePair { Size = 256, Percent = 0.35F },
                                                                    new SegmentSizePair { Size = 512, Percent = 0.35F },
                                                                    new SegmentSizePair { Size = 64, Percent = 0.1F });
            CodeTimer.Initialize();
            ISegmentCacheContainer<int> container = null;
            CodeTimer.Time("Initialize segment cache container", 1, delegate
            {
                //max power :)
               container = new SegmentCacheContainer<int>(850000000, policy);
            });
            Console.WriteLine("#Executing write cache test......");
            int failedCount = 0;
            CodeTimer.Time("Executing write cache test", 1, delegate
            {
                for (int i = 0; i < 800000; i++)
                {
                    byte[] data = new byte[randomNumbers[i]];
                    data[0] = 1;
                    data[data.Length - 1] = 2;
                    if(!container.Add(i, data))
                    {
                        failedCount++;
                        //Console.WriteLine(string.Format("-->#Write cache failed: id: {0}, size: {1}", i, data.Length));
                    }
                }
            });
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("#Total segment size: " + 850000000);
            Console.WriteLine("#Total cache size: " + totalSize);
            Console.WriteLine("#Waste percent: " + ((100 / 850000000D) * (850000000 - totalSize)) + "%");
            Console.WriteLine("#Failed: " + failedCount);
            Console.WriteLine("#Faild percent: " + (100 / 1000000D * failedCount) + "%");
            Console.WriteLine("Finished.");
            Console.ReadLine();
        }
    }
}
