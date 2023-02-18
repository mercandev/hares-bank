using System;
using HB.SharedObject;
using HB.SharedObject.CustomerViewModel;

namespace HB.Service.Process
{
	public interface IProcessService
	{
        Task<ReturnState<object>> BuyGold(ConvertMoneyToCoalViewModel model);
        Task<ReturnState<object>> CoalInformation();
    }
}

