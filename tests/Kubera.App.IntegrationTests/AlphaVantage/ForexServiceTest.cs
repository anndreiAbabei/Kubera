using System.Threading.Tasks;
using Xunit;

namespace Kubera.App.IntegrationTests.AlphaVantage
{
    public class ForexServiceTest : IClassFixture<AlphaVantageServiceFactory>
    {
        private readonly AlphaVantageServiceFactory _serviceFactory;

        public ForexServiceTest(AlphaVantageServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [Fact]
        public async Task GetForexPriceTest()
        {
            const string from = "USD";
            const string to = "EUR";

            var result = await _serviceFactory.Forex.GetPriceOf(from, to)
                .ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.Equal(from, result.Value.From);
            Assert.Equal(to, result.Value.To);
            Assert.NotEqual(0m, result.Value.Rate, 4);
        }
    }
}
