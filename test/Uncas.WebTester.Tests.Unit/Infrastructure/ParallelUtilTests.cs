namespace Uncas.WebTester.Tests.Unit.Infrastructure
{
    using System;
    using System.Threading;
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
                    {
                        sleepMilliseconds = random.Next(199, 201);
                    }

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
}
