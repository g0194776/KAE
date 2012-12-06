using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace KJFramework.Cache.ArithmeticTest
{
    class Program
    {
        private int[] _level = new[] { 1198, 679, 340, 64 };
        //private int[] _level = new[] { 2048, 960, 579, 64 };
        private static Random _random = new Random();
        private static int testCount = 2;
        private static int groupTest = 100000;
        static void Main(string[] args)
        {
            Program program = new Program();
            int size;
            List<int[]> intses;
            int total;
            int totalWaste = 0;
            int totalSaveBlocks = 0;
            for (int k = 0; k < groupTest; k++)
            {
                Console.WriteLine("#Begin group test: " + k);
                Console.ForegroundColor = ConsoleColor.Yellow;
                int[] numbers = new int[testCount];
                int[][] blocks = new int[testCount][];
                int[] localWaste = new int[testCount];
                for (int i = 0; i < testCount; i++)
                {
                    size = _random.Next(1, 99999);
                    numbers[i] = size;
                    intses = program.OrgGetSlot(size);
                    total = intses.Sum(inner => inner[0] * inner[1]);
                    Console.WriteLine("#Size: " + size);
                    Console.WriteLine("#Total: " + total);
                    Console.WriteLine("#Waste: " + (total - size));
                    localWaste[i] = total - size;
                    blocks[i] = new int[1];
                    //blocks[i][0] = intses.Sum(no => no[1]);
                    blocks[i] = intses.Select(no => no[1]).ToArray();
                    foreach (var intse in intses)
                    {
                        Console.WriteLine(string.Format("       #Block: {0}, Count: {1}", intse[0], intse[1]));
                    }
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.Green;
                int[][] nextBlocks = new int[testCount][];
                int[] nextlocalWaste = new int[testCount];
                for (int i = 0; i < testCount; i++)
                {
                    intses = program.GetSlot(numbers[i]);
                    total = intses.Sum(inner => inner[0] * inner[1]);
                    Console.WriteLine("#Size: " + numbers[i]);
                    Console.WriteLine("#Total: " + total);
                    Console.WriteLine("#Waste: " + (total - numbers[i]));
                    nextlocalWaste[i] = total - numbers[i];
                    nextBlocks[i] = new int[1];
                    nextBlocks[i] = intses.Select(no => no[1]).ToArray();
                    foreach (var intse in intses)
                    {
                        Console.WriteLine(string.Format("       #Block: {0}, Count: {1}", intse[0], intse[1]));
                    }
                    Console.WriteLine();
                }
                bool isChanged = false;
                for (int i = 0; i < testCount; i++)
                {
                    //if (blocks[i] != nextBlocks[i])
                    if (blocks[i].Length != nextBlocks[i].Length || !Compare(blocks[i], nextBlocks[i]))
                    {
                        isChanged = true;
                        totalWaste += (nextlocalWaste[i] - localWaste[i]);
                        totalSaveBlocks += (blocks[i].Sum() - nextBlocks[i].Sum());
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("#Found a different, index: " + i);
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                }
                if (isChanged)
                {
                    Console.WriteLine("#Total waste: " + totalWaste);
                    Console.WriteLine("#Total save blocks waste: " + totalSaveBlocks);
                    Console.ReadLine();
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Clear();
            }
            Console.WriteLine("#Begin performance test......");
            size = _random.Next(1, 534326);
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                program.GetSlot(size);
            }
            stopwatch.Stop();
            Console.WriteLine("#Performance test result: " + stopwatch.Elapsed);
            Console.ReadLine();
        }

        private static bool Compare(int[] arr1, int[] arr2)
        {
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }
            return true;
        }

        private List<int[]> GetSlot(int size)
        {
            List<int[]> result = new List<int[]>();
            int count;
            int lastSize;
            int subSize;
            foreach (int level in _level)
            {
                //优先选择靠近当前等级的分块
                if ((subSize = level - size) >= 0 && subSize <= _level[_level.Length - 1])
                {
                    result.Add(new[] { level, 1 });
                    return result;
                }
                if (size >= level)
                {
                    count = size / level;
                    lastSize = size - (level * count);
                    result.Add(new[] { level, count });
                    result.AddRange(GetSlot(lastSize));
                    return result;
                }
            }
            //小于所有的
            result.Add(new[] { _level[_level.Length - 1], 1 });
            return result;
        }

        private List<int[]> OrgGetSlot(int size)
        {
            List<int[]> result = new List<int[]>();
            int count;
            int lastSize;
            foreach (int level in _level)
            {
                if (size > level)
                {
                    count = size / level;
                    lastSize = size - (level * count);
                    result.Add(new[] { level, count });
                    result.AddRange(GetSlot(lastSize));
                    return result;
                }
            }
            //小于所有的
            result.Add(new[] { _level[_level.Length - 1], 1 });
            return result;
        }
    }
}
