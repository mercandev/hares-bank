using System;
using System.Numerics;
using System.Xml.Linq;
using AutoMapper;
using HB.Domain.Model;
using HB.SharedObject;
using HB.SharedObject.CustomerViewModel;

namespace HB.Service.Engine
{
	public class AutoMapperRegister : Profile
	{
        public AutoMapperRegister()
        {
            CreateMap<Accounts, AccountsViewModel>().ReverseMap();
            CreateMap<AccountsViewModel, Accounts>().ReverseMap();
        }
    }
}

