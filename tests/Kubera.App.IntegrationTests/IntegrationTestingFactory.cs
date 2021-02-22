using System;
using Bogus;
using Kubera.General;
using Kubera.General.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kubera.App.IntegrationTests
{
    public abstract class IntegrationTestingFactory
    {
        public virtual IAppSettings AppSettings { get; }

        public virtual ICacheService CacheService { get; }



        protected IntegrationTestingFactory()
        {
            var appMock = new Mock<IAppSettings>();
            var cacheMock = new Mock<ICacheService>();

            appMock.SetupGet(a => a.AlphaVantageApiKey)
                   .Returns(Environment.GetEnvironmentVariable("ALPHA_VANTAGE_API"));

            cacheMock.Setup(c => c.Get<It.IsAnyType>(It.IsAny<string>()))
                     .Returns(() => default);
            cacheMock.Setup(c => c.Add(It.IsAny<It.IsAnyType>(), It.IsAny<string>()));
            cacheMock.Setup(c => c.Remove<It.IsAnyType>(It.IsAny<string>()));
            cacheMock.Setup(c => c.RemoveAll<It.IsAnyType>());
            cacheMock.Setup(c => c.Clear());

            AppSettings = appMock.Object;
            CacheService = cacheMock.Object;
        }



        protected virtual ILogger<T> Logger<T>()
        {
            var logMock = new Mock<ILogger<T>>();

            return logMock.Object;
        }

        public virtual T CreateFake<T>(Action<Faker<T>> options = null)
            where T : class
        {
            var faker = new Faker<T>();

            options?.Invoke(faker);

            return faker.Generate();
        }
    }
}
