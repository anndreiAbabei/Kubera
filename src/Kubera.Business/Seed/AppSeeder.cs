using Kubera.Business.Entities.Defaults;
using Kubera.Business.Repository;
using Kubera.Data.Entities;
using Kubera.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Business.Seed
{
    public class AppSeeder : ISeeder
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IAssetRepository _assetRepository;

        public AppSeeder(IGroupRepository groupRepository, ICurrencyRepository currencyRepository, IAssetRepository assetRepository)
        {
            _groupRepository = groupRepository;
            _currencyRepository = currencyRepository;
            _assetRepository = assetRepository;
        }

        public async ValueTask Seed(CancellationToken cancellationToken = default)
        {
            var groups = await SeedGroup(cancellationToken).ConfigureAwait(false);
            await SeedCurrency(cancellationToken).ConfigureAwait(false);
            await SeedAssets(groups, cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask<IEnumerable<Group>> SeedGroup(CancellationToken cancellationToken = default)
        {
            IList<Group> groups = new List<Group>();

            foreach (var (Code, Name) in Codes.Group.Collection)
                if (cancellationToken.IsCancellationRequested)
                    return groups;
                else
                    groups.Add(await CreateGroupIfNotExists(Code, Name, cancellationToken));

            return groups;
        }

        private async ValueTask SeedCurrency(CancellationToken cancellationToken = default)
        {
            int index = 0;
            foreach (var (Code, Name, Symbol) in Codes.Currency.Collection)
                if (cancellationToken.IsCancellationRequested)
                    return;
                else
                    await CreateCurrencyIfNotExists(Code, Name, Symbol, index++, cancellationToken)
                        .ConfigureAwait(false);
        }


        private async ValueTask SeedAssets(IEnumerable<Group> groups, CancellationToken cancellationToken = default)
        {
            int index = 0;
            foreach (var (Code, Name, Symbol, GroupCode) in Codes.Asset.Collection)
                if (cancellationToken.IsCancellationRequested)
                    return;
                else
                    await CreateAssetsIfNotExists(Code, Name, Symbol, GroupCode, groups, index++, cancellationToken)
                        .ConfigureAwait(false);
        }

        private async ValueTask<Group> CreateGroupIfNotExists(string code, string name, CancellationToken cancellationToken = default)
        {
            var group = await _groupRepository.GetByCode(code)
                            .ConfigureAwait(false);

            if (group == null)
            {
                group = new Group
                {
                    Code = code,
                    Name = name,
                    CreatedAt = DateTime.UtcNow
                };

                group = await _groupRepository.Add(group, cancellationToken)
                    .ConfigureAwait(false);
            }

            return group;
        }

        private async ValueTask CreateCurrencyIfNotExists(string code, string name, string symbol, int index, CancellationToken cancellationToken = default)
        {
            var exCur = await _currencyRepository.GetByCode(code, cancellationToken)
                            .ConfigureAwait(false);

            if (exCur == null)
            {
                exCur = new Currency
                {
                    Code = code,
                    Name = name,
                    Symbol = symbol,
                    Order = index
                };

                await _currencyRepository.Add(exCur, cancellationToken);
            }
        }

        private async ValueTask CreateAssetsIfNotExists(string code, string name, string symbol, string groupCode, IEnumerable<Group> groups, int index, CancellationToken cancellationToken = default)
        {
            var curAsset = await _assetRepository.GetByCode(code, cancellationToken)
                            .ConfigureAwait(false);

            if (curAsset == null)
            {
                var group = groups.FirstOrDefault(g => g.Code == groupCode);

                curAsset = new Asset
                {
                    Code = code,
                    Name = name,
                    Symbol = symbol,
                    Order = index,
                    CreatedAt = DateTime.UtcNow,
                    GroupId = group.Id
                };

                await _assetRepository.Add(curAsset, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
