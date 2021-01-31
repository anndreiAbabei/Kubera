using Kubera.Business.Entities.Defaults;
using Kubera.Business.Repository;
using Kubera.Data.Entities;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Kubera.Business.Entities
{
    public interface IDefaultEntities
    {
        IDefaultGroups Groups { get; }
    }

    public class DefaultEntities : IDefaultEntities
    {
        public virtual IDefaultGroups Groups { get; }

        public DefaultEntities(IGroupRepository groupRepository)
        {
            Groups = new DefaultGroups(groupRepository);
        }

        private class DefaultGroups : IDefaultGroups
        {
            private readonly IGroupRepository _groupRepository;
            private readonly ConcurrentDictionary<string, Group> _dictionary;

            public DefaultGroups(IGroupRepository groupRepository)
            {
                _groupRepository = groupRepository;
                _dictionary = new ConcurrentDictionary<string, Group>();
            }

            public async ValueTask<Group> Commodity() => await GetAndOrAdd(Codes.Group.Commodity);

            public async ValueTask<Group> Crypto() => await GetAndOrAdd(Codes.Group.Crypto);

            public async ValueTask<Group> Stock() => await GetAndOrAdd(Codes.Group.Stock);

            private async ValueTask<Group> GetAndOrAdd(string code)
            {
                if (_dictionary.TryGetValue(code, out var group))
                    return group;

                group = await _groupRepository.GetByCode(code)
                    .ConfigureAwait(false);

                _dictionary.TryAdd(code, group);

                return group;
            }
        }
    }
}
