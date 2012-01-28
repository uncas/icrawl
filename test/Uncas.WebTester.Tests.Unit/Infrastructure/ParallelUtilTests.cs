namespace Uncas.WebTester.Tests.Unit.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class ParallelUtilTests
    {
        private static readonly object LockObject = new object();

        [Test]
        public void While_X()
        {
            int x = 0;
            var random = new Random();
            Func<bool> condition = () => x < 100;
            ParallelUtil.While(
                condition,
                () =>
                {
                    int sleepMilliseconds;
                    lock (LockObject)
                        sleepMilliseconds = random.Next(199, 201);
                    Thread.Sleep(sleepMilliseconds);
                    lock (LockObject)
                    {
                        x++;
                        Console.WriteLine(x);
                    }
                },
                10);
        }
    }

    public class ParallelUtil
    {
        public static void While(
            Func<bool> condition,
            Action body,
            int maxDegreeOfParallelism)
        {
            var parallelOptions =
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                };
            Parallel.ForEach(
                IterateUntilFalse(condition),
                parallelOptions,
                ignored => body());
        }

        private static IEnumerable<bool> IterateUntilFalse(Func<bool> condition)
        {
            while (condition())
                yield return true;
        }
    }
}
