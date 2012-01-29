namespace Uncas.WebTester.Tests.Unit.Infrastructure
{
    using System;
    using System.Threading;
    using NUnit.Framework;
    using System.Diagnostics;

    [TestFixture]
    public class ParallelUtilTests
    {
        private static readonly object LockObject = new object();

        [Test]
        public void While_X()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            int x = 0;
            var random = new Random();
            Func<bool> condition = () => x < 20;
            ParallelUtil.While(
                condition,
                () =>
                {
                    int sleepMilliseconds;
                    lock (LockObject)
                    {
                        if (!condition())
                            return;
                        sleepMilliseconds = random.Next(19, 21);
                    }

                    Thread.Sleep(sleepMilliseconds);
                    lock (LockObject)
                    {
                        if (!condition())
                            return;
                        x++;
                        Console.WriteLine(x);
                    }
                },
                10);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            // 2: 1409, 1306, 1346, 1305, 1314, 1305, 1311, 1306
            // 4: 739, 759, 905, 704, 707, 703, 752, 705
            // 10: 514, 849, 905, 503, 505, 711, 704, 503, 509
        }

        [Test]
        public void ParallelCrawl_Pseudo()
        {
            // Determine max pages to visit (maxVisits).
            // Determine how many to run in parallel (maxDegreeOfParallelism).
            // Serially: create list of 2*maxDegreeOfParallelism pages to visit (links).
            // Parallel:
            // - For x = 0, x < maxVisits, x++.
            // - Lock: get random non-visited link from list and set visited=true.
            // - Visit page and extract details.
            // - Lock:
            //   - Add details about the link/page and add new-found links.
            //   - visits++
        }
    }
}
