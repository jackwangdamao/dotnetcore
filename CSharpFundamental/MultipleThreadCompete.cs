using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpFundamental
{
    class MultipleThreadCompete
    {
        List<int> numList = new List<int>();
        Random random = new Random();
        CancellationTokenSource cts = new CancellationTokenSource();
        private const int TOTAL_NUM = 10000000;
        private const int CURRENT_THREAD_COUNT = 10;

        ReaderWriterLockSlim rwls = new ReaderWriterLockSlim();

        int[] bitMap = new int[TOTAL_NUM];

        internal void DoTheCompeteSecond()
        {
            //ThreadPool.SetMinThreads(CURRENT_THREAD_COUNT, CURRENT_THREAD_COUNT);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Task[] tasks = new Task[CURRENT_THREAD_COUNT];

            for (int i = 0; i < CURRENT_THREAD_COUNT; ++i)
            {
                int num = i;
                tasks[i] = Task.Run(() => ExecuteTheTask(num, cts), cts.Token);
            }

            Task.WaitAny(tasks);

            Console.WriteLine("ExecuteTime={0}", sw.ElapsedMilliseconds);
        }

        private int GetTheListCount()
        {
            return numList.Count;
        }

        private void AddToList(int num)
        {
            if (numList.Count == TOTAL_NUM)
                return;
            numList.Add(num);

            var index = num % TOTAL_NUM;
            bitMap[index] = 1;
        }

        private void ExecuteTheTask(object state, CancellationTokenSource cts)
        {
            //Console.WriteLine("This is the {0} thread,Current ThreadId={1}",
            //    state,
            //    Thread.CurrentThread.ManagedThreadId);

            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    rwls.EnterReadLock();
                    if (numList.Count == TOTAL_NUM)
                    {
                        cts.Cancel();
                        Console.WriteLine("Current Thread Id={0},Current Count={1}",
                            Thread.CurrentThread.ManagedThreadId,
                            GetTheListCount());
                        break;
                    }
                }
                finally
                {
                    rwls.ExitReadLock();
                }

                var insertNum = GenerateInt32Num();

                rwls.EnterReadLock();
                if (NumContains(insertNum))
                {
                    insertNum = GenerateInt32Num();
                }
                rwls.ExitReadLock();

                rwls.EnterWriteLock();
                AddToList(insertNum);
                rwls.ExitWriteLock();
            }
        }

        private int GenerateInt32Num()
        {
            var num = random.Next(0, TOTAL_NUM);
            //Console.WriteLine(num);
            return num;
        }

        private bool NumContains(int num)
        {
            return bitMap[num] == 1;
        }
    }
}