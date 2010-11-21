namespace Uncas.WebTester.Tests.Unit.Mocks
{
    using Uncas.WebTester.ApplicationServices;
    using Uncas.WebTester.Models;

    internal class ResultServiceMock : IResultService
    {
        private int numberOfResults = 0;

        internal int NumberOfResults
        {
            get
            {
                return this.numberOfResults;
            }
        }

        public void ProcessResult(HyperLink link)
        {
            this.numberOfResults++;
        }
    }
}
