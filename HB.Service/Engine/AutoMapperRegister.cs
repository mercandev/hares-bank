using System;
using System.Numerics;
using System.Xml.Linq;
using AutoMapper;
using HB.Domain.Model;
using HB.SharedObject;

namespace HB.Service.Engine
{
	public class AutoMapperRegister : Profile
	{
        #region Constructors

       
        public AutoMapperRegister()
        {
            CreateMap<Accounts, AccountsViewModel>().ReverseMap();
            CreateMap<AccountsViewModel, Accounts>().ReverseMap();
        }

        #endregion
    }
}

