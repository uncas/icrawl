namespace Uncas.WebTester.Tests.Unit.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class ParallelUtilTests
    {
        private static readonly object lockObject = new object();

        private static Random random = new Random();

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
                    lock (lockObject)
                    {
                        if (!condition())
                        {
                            return;
                        }

                        sleepMilliseconds = random.Next(19, 21);
                    }

                    Thread.Sleep(sleepMilliseconds);
                    lock (lockObject)
                    {
                        if (!condition())
                        {
                            return;
                        }

                        x++;
                        Console.WriteLine(x);
                    }
                },
                10);
            stopwatch.Stop();

            // 2: 1409, 1306, 1346, 1305, 1314, 1305, 1311, 1306
            // 4: 739, 759, 905, 704, 707, 703, 752, 705
            // 10: 514, 849, 905, 503, 505, 711, 704, 503, 509
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        [Test]
        public void ParallelCrawl_Pseudo()
        {
            // Determine max pages to visit (maxVisits).
            int maxPages = 100;

            // Determine how many to run in parallel (maxDegreeOfParallelism).
            int maxDegreeOfParallelism = 5;

            // Serially: create list of 2*maxDegreeOfParallelism pages to visit (links).
            var list = new List<PageDummy>(maxPages);
            
            // Adding initial pages:
            list.AddRange(this.GetInitialPages());
            int numberOfInitialPages = list.Count;

            // TODO: Warm initial pages up
            // TODO: Pre-populate list by visiting initial pages:
            for (int i = 0; i < (2 * maxDegreeOfParallelism) - numberOfInitialPages; i++)
            {
                list.Add(new PageDummy(random.Next()));
            }

            // Parallel:
            // - For x = 0, x < maxVisits, x++.
            var options =
                new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };
            int visits = 0;
            Parallel.For(
                0,
                maxPages,
                options,
                (x) =>
                {
                    var page = GetRandomPage(list);
                    var details = GetPageDetails(page);
                    AddDetails(list, details);
                    lock (lockObject)
                    {
                        visits++;
                    }

                    OutputDetails(page, visits);
                });
        }

        private static void AddDetails(List<PageDummy> list, PageDetails details)
        {
            // Add details about the link/page and add new-found links.
            lock (lockObject)
            {
                list.AddRange(details.Links);
            }
        }

        private IEnumerable<PageDummy> GetInitialPages()
        {
            var result = new List<PageDummy>();
            result.Add(new PageDummy(random.Next()));
            result.Add(new PageDummy(random.Next()));
            return result;
        }

        private void OutputDetails(PageDummy page, int visits)
        {
            Console.WriteLine("{0}: {1}", visits, page.Id);
        }

        private PageDetails GetPageDetails(PageDummy page)
        {
            // Visit page and extract details.
            // Simulating some time to visit the page:
            Thread.Sleep(10);
            var result = new PageDetails();
            result.AddLink();
            result.AddLink();
            result.AddLink();
            result.AddLink();
            return result;
        }

        private PageDummy GetRandomPage(List<PageDummy> list)
        {
            // Get random non-visited link from list and set visited=true.
            lock (lockObject)
            {
                var unvisited = list.Where(x => !x.Visited);
                var count = unvisited.Count();
                var index = random.Next(count);
                var item = unvisited.ElementAt(index);
                item.Visited = true;
                return item;
            }
        }
    }
}