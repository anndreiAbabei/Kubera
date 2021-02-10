using Kubera.App.IntegrationTests.TestData;
using Kubera.App.Models;
using Kubera.Data.Entities;
using Xunit;

namespace Kubera.App.IntegrationTests.Misc
{
    public class MappingTests : IClassFixture<MiscTestingFactory>
    {
        private readonly MiscTestingFactory _factory;

        public MappingTests(MiscTestingFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public void MapGroupToGroupModelTest()
        {
            CreateAndAssertIsTheSame<Group, GroupModel>();
        }

        [Fact]
        public void MapAssetToAssetModelTest()
        {
            CreateAndAssertIsTheSame<Asset, AssetModel>();
        }

        [Fact]
        public void MapCurrencyToCurrencyModelTest()
        {
            CreateAndAssertIsTheSame<Currency, CurrencyModel>();
        }

        [Fact]
        public void MapTransactionToTransactionModelTest()
        {
            CreateAndAssertIsTheSame<Transaction, TransactionModel>();
        }

        private void CreateAndAssertIsTheSame<TEntity, TModel>(IEqualiable<TModel, TEntity> equaliable = null)
            where TEntity : class
            where TModel : class
        {
            var entity = _factory.CreateFake<TEntity>();
            var model = _factory.Mapper.Map<TEntity, TModel>(entity);

            equaliable ??= new GenericEqualiable<TModel, TEntity>();

            Assert.True(equaliable.AreEqual(model, entity));
        }
    }
}
