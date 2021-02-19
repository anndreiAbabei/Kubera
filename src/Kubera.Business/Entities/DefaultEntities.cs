using Kubera.Application.Services;
using Kubera.Business.Entities.Defaults;
using Kubera.Data.Entities;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Kubera.General.Defaults;

namespace Kubera.Business.Entities
{
    public interface IDefaultEntities
    {
        IDefaultGroups Groups { get; }
    }

    public class DefaultEntities : IDefaultEntities, IDefaults
    {
        private readonly DefaultGroups _groups;

        public virtual string Currency => "EUR";

        IDefaultGroupsCodes IDefaults.Groups => _groups;

        IDefaultGroups IDefaultEntities.Groups => _groups;

        public DefaultEntities(IGroupRepository groupRepository)
        {
            _groups = new DefaultGroups(groupRepository);
        }

        private class DefaultGroups : IDefaultGroups, IDefaultGroupsCodes
        {
            private readonly IGroupRepository _groupRepository;
            private readonly ConcurrentDictionary<string, Group> _dictionary;

            public DefaultGroups(IGroupRepository groupRepository)
            {
                _groupRepository = groupRepository;
                _dictionary = new ConcurrentDictionary<string, Group>();
            }

            public string Commodity => Codes.Group.Commodity;

            public string Crypto => Codes.Group.Crypto;

            public string Stock => Codes.Group.Stock;

            public async ValueTask<Group> GetCommodity() => await GetAndOrAdd(Commodity);

            public async ValueTask<Group> GetCrypto() => await GetAndOrAdd(Crypto);

            public async ValueTask<Group> GetStock() => await GetAndOrAdd(Stock);

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
