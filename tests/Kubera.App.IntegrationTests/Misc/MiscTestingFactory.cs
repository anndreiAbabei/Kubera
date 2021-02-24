using AutoMapper;
using Kubera.App.Mapper;

namespace Kubera.App.IntegrationTests.Misc
{
    public class MiscTestingFactory : IntegrationTestingFactory
    {
        public virtual IMapper Mapper { get; }

        public MiscTestingFactory()
        {
            Mapper = new MapperConfiguration(c => c.AddProfile<AppMapper>())
                .CreateMapper();
        }
    }
}
