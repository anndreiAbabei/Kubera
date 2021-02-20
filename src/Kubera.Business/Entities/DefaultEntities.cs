using Kubera.Business.Entities.Defaults;
using Kubera.Data.Entities;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Kubera.General.Defaults;
using Kubera.Data.Store;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;

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

        public DefaultEntities(IGroupStore groupStore)
        {
            _groups = new DefaultGroups(groupStore);
        }

        private class DefaultGroups : IDefaultGroups, IDefaultGroupsCodes
        {
            private readonly IGroupStore _groupStore;
            private readonly ConcurrentDictionary<string, Group> _dictionary;

            public DefaultGroups(IGroupStore groupStore)
            {
                _groupStore = groupStore;
                _dictionary = new ConcurrentDictionary<string, Group>();
            }

            public string Commodity => Codes.Group.Commodity;

            public string Crypto => Codes.Group.Crypto;

            public string Stock => Codes.Group.Stock;

            public async ValueTask<Group> GetCommodity(CancellationToken canellationToken = default) => await GetAndOrAdd(Commodity, canellationToken);

            public async ValueTask<Group> GetCrypto(CancellationToken canellationToken = default) => await GetAndOrAdd(Crypto, canellationToken);

            public async ValueTask<Group> GetStock(CancellationToken canellationToken = default) => await GetAndOrAdd(Stock, canellationToken);

            private async ValueTask<Group> GetAndOrAdd(string code, CancellationToken canellationToken = default)
            {
                if (_dictionary.TryGetValue(code, out var group))
                    return group;

                group = await _groupStore.GetAll()
                    .Where(g => g.Code == code)
                    .FirstOrDefaultAsync(canellationToken)
                    .ConfigureAwait(false);

                _dictionary.TryAdd(code, group);

                return group;
            }
        }
    }
}
