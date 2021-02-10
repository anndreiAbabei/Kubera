using AutoMapper;
using Kubera.Application.Common.Models;
using Kubera.Business.Entities;
using Kubera.Data.Entities;

namespace Kubera.App.Mapper
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<Group, GroupModel>()
                .ForMember(gm => gm.Id, c => c.MapFrom(g => g.Id))
                .ForMember(gm => gm.Code, c => c.MapFrom(g => g.Code))
                .ForMember(gm => gm.Name, c => c.MapFrom(g => g.Name));

            CreateMap<Currency, CurrencyModel>()
                .ForMember(cm => cm.Code, c => c.MapFrom(c => c.Code))
                .ForMember(cm => cm.Name, c => c.MapFrom(c => c.Name))
                .ForMember(cm => cm.Symbol, c => c.MapFrom(c => c.Symbol))
                .ForMember(cm => cm.Order, c => c.MapFrom(c => c.Order));

            CreateMap<Asset, AssetModel>()
                .ForMember(am => am.Id, c => c.MapFrom(a => a.Id))
                .ForMember(am => am.Code, c => c.MapFrom(a => a.Code))
                .ForMember(am => am.Name, c => c.MapFrom(a => a.Name))
                .ForMember(am => am.Symbol, c => c.MapFrom(a => a.Symbol))
                .ForMember(am => am.Order, c => c.MapFrom(a => a.Order))
                .ForMember(am => am.Icon, c => c.MapFrom(a => a.Icon))
                .ForMember(am => am.GroupId, c => c.MapFrom(a => a.GroupId));

            CreateMap<Transaction, TransactionModel>()
                .ForMember(tm => tm.Id, c => c.MapFrom(t => t.Id))
                .ForMember(tm => tm.AssetId, c => c.MapFrom(t => t.AssetId))
                .ForMember(tm => tm.Wallet, c => c.MapFrom(t => t.Wallet))
                .ForMember(tm => tm.CreatedAt, c => c.MapFrom(t => t.CreatedAt))
                .ForMember(tm => tm.Amount, c => c.MapFrom(t => t.Amount))
                .ForMember(tm => tm.CurrencyId, c => c.MapFrom(t => t.CurrencyId))
                .ForMember(tm => tm.Rate, c => c.MapFrom(t => t.Rate))
                .ForMember(tm => tm.Fee, c => c.MapFrom(t => t.Fee))
                .ForMember(tm => tm.FeeCurrencyId, c => c.MapFrom(t => t.FeeCurrencyId));

            CreateMap<UserSettings, UserSettingsModel>()
                .ForMember(usm => usm.Language, c => c.MapFrom(us => us.Language))
                .ForMember(usm => usm.PrefferedCurrency, c => c.MapFrom(us => us.PrefferedCurrency))
                .ForMember(usm => usm.Currencies, c => c.MapFrom(us => us.Currencies));
        }
    }
}
