using System;
using HB.SharedObject;
using HB.SharedObject.PaymentViewModel;

namespace HB.Service.File
{
	public interface IFileService
	{
        Task<ReturnState<object>> GenerateReceiptPdf(Guid transactionsId);
        Task<ReturnState<object>> TransactionsHtml(Guid transactionsId);
    }
}

