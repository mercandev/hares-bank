using System;
using System.Numerics;
using System.Xml.Linq;
using AutoMapper;
using HB.Domain.Model;
using HB.Service.Helpers;
using HB.SharedObject;
using HB.SharedObject.CardViewModel;
using HB.SharedObject.CustomerViewModel;
using HB.SharedObject.ExchangeViewModel;
using HB.SharedObject.PaymentViewModel;
using HB.SharedObject.TransactionViewModel;

namespace HB.Service.Engine
{
	public class AutoMapperRegister : Profile
	{
        public AutoMapperRegister()
        {
            //Accounts
            CreateMap<Accounts, AccountsViewModel>().ReverseMap();
            CreateMap<AccountsViewModel, Accounts>().ReverseMap();

            //Cards
            CreateMap<CardGeneratorViewModel, Cards>().ReverseMap();
            CreateMap<Cards, CardGeneratorViewModel>().ReverseMap();
            CreateMap<Cards, EmptyCardsViewModel>()
                .ForMember(dest => dest.CardPaymentType, from => from.MapFrom(s => s.CardPaymentType.GetEnumDescription()));

            //Transactions
            CreateMap<Transactions, TransactionsResponseViewModel>()
                .ForMember(dest => dest.TransactionsType, from => from.MapFrom(s => s.TransactionsType.GetEnumDescription()))
                .ForMember(dest => dest.ProccessType, from => from.MapFrom(s => s.ProccessType.GetEnumDescription()));

            //Payment
            CreateMap<Organisations, OrganisationsViewModel>()
                .ForMember(dest => dest.OrganisationType, from => from.MapFrom(s => s.OrganisationType.GetEnumDescription()));

            //Excange
            CreateMap<CurrencyDetail, ExchangeMappingResponseViewModel>()
                .ForMember(dest => dest.Selling, from => from.MapFrom(s => s.satis))
                .ForMember(dest => dest.Buying, from => from.MapFrom(s => s.alis))
                .ForMember(dest => dest.Changing, from => from.MapFrom(s => s.degisim))
                .ForMember(dest => dest.ChangingRate, from => from.MapFrom(s => s.d_oran))
                .ForMember(dest => dest.ChangeingRoute, from => from.MapFrom(s => s.d_yon));
        }
    }
}

