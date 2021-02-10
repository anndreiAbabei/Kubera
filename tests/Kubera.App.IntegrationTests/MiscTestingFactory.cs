using System;
using AutoMapper;
using Bogus;
using Kubera.App.Mapper;

namespace Kubera.App.IntegrationTests
{
    public class MiscTestingFactory
    {
        internal virtual IMapper Mapper { get; }

        public MiscTestingFactory()
        {
            Mapper = new MapperConfiguration(c => c.AddProfile<AppMapper>())
                .CreateMapper();
        }

        internal T CreateFake<T>(Action<Faker<T>> options = null)
            where T : class
        {
            var faker = new Faker<T>();

            options?.Invoke(faker);

            return faker.Generate();
        }
    }
}
