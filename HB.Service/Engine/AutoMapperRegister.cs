using System;
using System.Numerics;
using System.Xml.Linq;
using AutoMapper;
using HB.Domain.Model;
using HB.Service.Helpers;
using HB.SharedObject;
using HB.SharedObject.CardViewModel;
using HB.SharedObject.CustomerViewModel;

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
               
        }
    }
}

