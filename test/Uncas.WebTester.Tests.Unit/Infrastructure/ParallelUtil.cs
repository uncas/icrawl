namespace Uncas.WebTester.Tests.Unit.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
            {
                yield return true;
            }
        }
    }
}
