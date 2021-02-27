using Kubera.Application.Services;
using Kubera.Business.Entities.Defaults;
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

            foreach ((string code, string name) in Codes.Group.Collection)
                if (cancellationToken.IsCancellationRequested)
                    return await Task.FromCanceled<IEnumerable<Group>>(cancellationToken);
                else
                    groups.Add(await CreateGroupIfNotExists(code, name, cancellationToken));

            return groups;
        }

        private async ValueTask SeedCurrency(CancellationToken cancellationToken = default)
        {
            int index = 0;
            foreach ((string code, string name, string symbol) in Codes.Currency.Collection)
                if (cancellationToken.IsCancellationRequested)
                    return;
                else
                    await CreateCurrencyIfNotExists(code, name, symbol, index++, cancellationToken)
                        .ConfigureAwait(false);
        }



        private async ValueTask SeedAssets(IEnumerable<Group> groups, CancellationToken cancellationToken = default)
        {
            int index = 0;
            var grList = groups as IList<Group> ?? groups.ToList();
            foreach ((string code, string name, string symbol, string groupCode) in Codes.Asset.Collection)
                if (cancellationToken.IsCancellationRequested)
                    return;
                else
                    await CreateAssetsIfNotExists(code, name, symbol, groupCode, grList, index++, cancellationToken)
                       .ConfigureAwait(false);
        }



        private async ValueTask<Group> CreateGroupIfNotExists(string code, string name, CancellationToken cancellationToken = default)
        {
            var group = await _groupRepository.GetByCode(code, cancellationToken)
                            .ConfigureAwait(false);

            if (group != null)
                return group;

            group = new Group
                    {
                        Code = code,
                        Name = name,
                        CreatedAt = DateTime.UtcNow
                    };

            return await _groupRepository.Add(group, cancellationToken)
                                         .ConfigureAwait(false);
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

                if(group == null)
                    return;

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
