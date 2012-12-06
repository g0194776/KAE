using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Test;
using KJFramework.Timer;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryAllotter.Instance.Initialize();
            BuildFullSerializeTestResult();
            BuildFullDeSerializeTestResult();
            Console.WriteLine("Finished.");
            Console.ReadLine();
        }

        public void Test()
        {
            TestObject objects = new TestObject
            {
                Uu = new string[] {null, null},
                Pp = new[] {null, new TestObject1 {Haha = "..."}},
                Jj = new[] {"Kevin", null, "Jee"},
                Mm = new[] {9988, 9999},
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 {Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow},
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            CodeTimer.Initialize();
            CodeTimer.Time("Bind object test: 100000", 1, delegate
            {
                for (int i = 0; i < 100000; i++) objects.Bind();
            });
            //Console.WriteLine();
            //CodeTimer.Initialize();
            //CodeTimer.Time("Pickup object test: 100000", 1, delegate
            //{
            //    for (int i = 0; i < 100000; i++)
            //    {
            //        TestObject ok = IntellectObjectEngine.GetObject<TestObject>(t, objects.Body);
            //        if (!ok.IsPickup) Console.WriteLine("Pickup object failed.");
            //    }
            //});
        }

        public void Test2()
        {
            DateTime tt = DateTime.Now;
            TestObject testObject = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Mm = new[] { 9988, 9999 },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = tt,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            CodeTimer.Initialize();
            CodeTimer.Time("ExtremeComplexObjectToStringTest", 1, delegate
            {
                for (int i = 0; i < 100000; i++)
                    testObject.ToString();
            });
            Console.WriteLine(testObject.ToString());
        }


        public void Test3()
        {
            Type t = typeof(OneFieldMessage);
            IntellectObjectEngine.Preheat(new OneFieldMessage());
            OneFieldMessage objects = new OneFieldMessage();
            Console.WriteLine("Initialize 1000000 objects successed.");
            CodeTimer.Initialize();
            CodeTimer.Time("##OneFieldMessage Bind object test: 1000000", 1, delegate
            {
                for (int i = 0; i < 1000000; i++) objects.Bind();
            });
            Console.WriteLine();
            CodeTimer.Initialize();
            CodeTimer.Time("##OneFieldMessage Pickup object test: 1000000", 1, delegate
            {
                for (int i = 0; i < 1000000; i++)
                {
                    OneFieldMessage ok = IntellectObjectEngine.GetObject<OneFieldMessage>(t, objects.Body);
                    if (!ok.IsPickup) Console.WriteLine("Pickup object failed.");
                }
            });
        }

        public static void BuildFullTestResult()
        {
            
        }

        private static void BuildFullDeSerializeTestResult()
        {
            int interactor = 100000;

            #region Build head.

            Console.WriteLine("BUILD FULL DESERIALIZTION TEST RESULT:");
            Console.WriteLine(new string('-', 20));
            Console.Write("Test Name");
            Console.Write(new string(' ', 20));
            Console.Write("Interator");
            Console.Write(new string(' ', 5));
            Console.Write("G0");
            Console.Write(new string(' ', 5));
            Console.Write("G1");
            Console.Write(new string(' ', 5));
            Console.Write("G2");
            Console.Write(new string(' ', 5));
            Console.WriteLine("Elapsed");

            #endregion

            #region Build normal array test.

            TestObject obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Mm = new[] { 9988, 9999 },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            int[] generations = ClearGCState();
            Stopwatch watcher = Stopwatch.StartNew();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("Normal Array(Bind)");
            Console.Write(new string(' ', 11));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Pickup normal array test.

            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < 100000; i++)
            {
                TestObject ok = IntellectObjectEngine.GetObject<TestObject>(obj.Body);
                if (!ok.IsPickup) Console.WriteLine("Pickup object failed.");
            }
            watcher.Stop();
            Console.Write("Normal Array(Pickup)");
            Console.Write(new string(' ', 9));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#System.Int32 Array Pickup Performance Test...");
            Console.ForegroundColor = ConsoleColor.Gray;

            #region Build many array element(100) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Mm = new int[100];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Mm[i] = i;
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(100)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(200) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Mm = new int[200];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Mm[i] = i;
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(200)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(500) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Mm = new int[500];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Mm[i] = i;
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(500)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(1000) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Mm = new int[1000];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Mm[i] = i;
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(1000)");
            Console.Write(new string(' ', 1));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#System.String Array Pickup Performance Test...");
            Console.ForegroundColor = ConsoleColor.Gray;

            #region Build many array element(100) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Jj = new string[100];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Jj[i] = "kevin";
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(100)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(200) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Jj = new string[200];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Jj[i] = "kevin";
            generations = ClearGCState();
            watcher.Restart();
            obj.Bind();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(200)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(500) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Jj = new string[500];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Jj[i] = "kevin";
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(500)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(1000) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Jj = new string[1000];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Jj[i] = "kevin";
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(1000)");
            Console.Write(new string(' ', 1));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#System.Int64 Array Pickup Performance Test...");
            Console.ForegroundColor = ConsoleColor.Gray;

            #region Build many array element(100) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Longs = new long[100];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Longs[i] = i;
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(100)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(200) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Longs = new long[200];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Longs[i] = i;
            generations = ClearGCState();
            watcher.Restart();
            obj.Bind();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(200)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(500) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Longs = new long[500];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Longs[i] = i;
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(500)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(1000) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Longs = new long[1000];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Longs[i] = i;
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Many Array Element(1000)");
            Console.Write(new string(' ', 1));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#System.String Pickup Performance Test...");
            Console.ForegroundColor = ConsoleColor.Gray;

            #region Build long string(100) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Obj = new TestObject1 { Haha = new string('*', 100), Colors = Colors.Yellow };
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Long String(100)");
            Console.Write(new string(' ', 9));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build long string(200) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Obj = new TestObject1 { Haha = new string('*', 200), Colors = Colors.Yellow };
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Long String(200)");
            Console.Write(new string(' ', 9));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build long string(500) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Obj = new TestObject1 { Haha = new string('*', 500), Colors = Colors.Yellow };
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Long String(500)");
            Console.Write(new string(' ', 9));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build long string(1000) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Obj = new TestObject1 { Haha = new string('*', 1000), Colors = Colors.Yellow };
            generations = ClearGCState();
            obj.Bind();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) IntellectObjectEngine.GetObject<TestObject>(obj.Body);
            watcher.Stop();
            Console.Write("    Long String(1000)");
            Console.Write(new string(' ', 8));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#One Field Pickup Performance Test...");
            Console.ForegroundColor = ConsoleColor.Gray;

            #region One Field(100000)

            OneFieldMessage msg = new OneFieldMessage { X = 10 };
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < 100000; i++) msg.Bind();
            watcher.Stop();
            Console.Write("    One Field(100000)");
            Console.Write(new string(' ', 8));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region One Field(1000000)

            msg = new OneFieldMessage { X = 10 };
            generations = ClearGCState();
            msg.Bind();
            watcher.Restart();
            for (int i = 0; i < 1000000; i++) IntellectObjectEngine.GetObject<OneFieldMessage>(msg.Body);
            watcher.Stop();
            Console.Write("    One Field(1000000)");
            Console.Write(new string(' ', 7));
            Console.Write(1000000);
            Console.Write(new string(' ', 7));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion
        }

        private static void BuildFullSerializeTestResult()
        {
            int interactor = 100000;

            #region Build head.

            Console.WriteLine("BUILD FULL SERIALIZTION TEST RESULT:");
            Console.WriteLine(new string('-', 20));
            Console.Write("Test Name");
            Console.Write(new string(' ', 20));
            Console.Write("Interator");
            Console.Write(new string(' ', 5));
            Console.Write("G0");
            Console.Write(new string(' ', 5));
            Console.Write("G1");
            Console.Write(new string(' ', 5));
            Console.Write("G2");
            Console.Write(new string(' ', 5));
            Console.WriteLine("Elapsed");

            #endregion

            #region Build normal array test.

            TestObject obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Mm = new[] { 9988, 9999 },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            int[] generations = ClearGCState();
            Stopwatch watcher = Stopwatch.StartNew();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("Normal Array(Bind)");
            Console.Write(new string(' ', 11));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Pickup normal array test.

            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < 100000; i++)
            {
                TestObject ok = IntellectObjectEngine.GetObject<TestObject>(obj.Body);
                if (!ok.IsPickup) Console.WriteLine("Pickup object failed.");
            }
            watcher.Stop();
            Console.Write("Normal Array(Pickup)");
            Console.Write(new string(' ', 9));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#System.Int32 Array Bind Performance Test...");
            Console.ForegroundColor = ConsoleColor.Gray;

            #region Build many array element(100) test.

            obj = new TestObject
                {
                    Uu = new string[] {null, null},
                    Pp = new[] {null, new TestObject1 {Haha = "..."}},
                    Jj = new[] {"Kevin", null, "Jee"},
                    Wocao = 111111,
                    Wokao = 222222,
                    Metadata1 = 99,
                    Metadata = Encoding.Default.GetBytes("haha"),
                    Time = DateTime.Now,
                    Obj = new TestObject1 {Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow},
                    NullableValue1 = 10,
                    NullableValue3 = 16
                };
            obj.Mm = new int[100];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Mm[i] = i;
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(100)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(200) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Mm = new int[200];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Mm[i] = i;
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(200)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(500) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Mm = new int[500];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Mm[i] = i;
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(500)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(1000) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Mm = new int[1000];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Mm[i] = i;
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(1000)");
            Console.Write(new string(' ', 1));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#System.String Array Bind Performance Test...");
            Console.ForegroundColor = ConsoleColor.Gray;

            #region Build many array element(100) test.

            obj = new TestObject
            {
                Uu = new string[] {null, null},
                Pp = new[] {null, new TestObject1 {Haha = "..."}},
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] {9999, 9988},
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 {Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow},
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Jj = new string[100];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Jj[i] = "kevin";
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(100)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(200) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Jj = new string[200];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Jj[i] = "kevin";
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(200)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(500) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Jj = new string[500];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Jj[i] = "kevin";
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(500)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(1000) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Jj = new string[1000];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Jj[i] = "kevin";
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(1000)");
            Console.Write(new string(' ', 1));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#System.Int64 Array Bind Performance Test...");
            Console.ForegroundColor = ConsoleColor.Gray;

            #region Build many array element(100) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Longs = new long[100];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Longs[i] = i;
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(100)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(200) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Longs = new long[200];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Longs[i] = i;
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(200)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(500) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Longs = new long[500];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Longs[i] = i;
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(500)");
            Console.Write(new string(' ', 2));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build many array element(1000) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Longs = new long[1000];
            for (int i = 0; i < obj.Mm.Length; i++) obj.Longs[i] = i;
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Many Array Element(1000)");
            Console.Write(new string(' ', 1));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#System.String Performance Test...");
            Console.ForegroundColor = ConsoleColor.Gray;

            #region Build long string(100) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Obj = new TestObject1 {Haha = new string('*', 100), Colors = Colors.Yellow};
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Long String(100)");
            Console.Write(new string(' ', 9));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build long string(200) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Obj = new TestObject1 { Haha = new string('*', 200), Colors = Colors.Yellow };
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Long String(200)");
            Console.Write(new string(' ', 9));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build long string(500) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Obj = new TestObject1 { Haha = new string('*', 500), Colors = Colors.Yellow };
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Long String(500)");
            Console.Write(new string(' ', 9));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region Build long string(1000) test.

            obj = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Mm = new[] { 9999, 9988 },
                Jj = new[] { "kevin", null, "kline" },
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = DateTime.Now,
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            obj.Obj = new TestObject1 { Haha = new string('*', 1000), Colors = Colors.Yellow };
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < interactor; i++) obj.Bind();
            watcher.Stop();
            Console.Write("    Long String(1000)");
            Console.Write(new string(' ', 8));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 4));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("#One Field Performance Test...");
            Console.ForegroundColor = ConsoleColor.Gray;

            #region One Field(100000)

            OneFieldMessage msg = new OneFieldMessage {X = 10};
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < 100000; i++) msg.Bind();
            watcher.Stop();
            Console.Write("    One Field(100000)");
            Console.Write(new string(' ', 8));
            Console.Write(interactor);
            Console.Write(new string(' ', 8));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion

            #region One Field(1000000)

            msg = new OneFieldMessage { X = 10 };
            generations = ClearGCState();
            watcher.Restart();
            for (int i = 0; i < 1000000; i++) msg.Bind();
            watcher.Stop();
            Console.Write("    One Field(1000000)");
            Console.Write(new string(' ', 7));
            Console.Write(1000000);
            Console.Write(new string(' ', 7));
            Console.Write(GC.CollectionCount(0) - generations[0]);
            Console.Write(new string(' ', 5));
            Console.Write(GC.CollectionCount(1) - generations[1]);
            Console.Write(new string(' ', 6));
            Console.Write(GC.CollectionCount(2) - generations[2]);
            Console.Write(new string(' ', 6));
            Console.WriteLine(watcher.Elapsed.TotalMilliseconds + " ms");

            #endregion
        }

        private static int[] ClearGCState()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
                gcCounts[i] = GC.CollectionCount(i);
            return gcCounts;
        }
    }

    public class OneFieldMessage : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public int X { get; set; }
    }
}
