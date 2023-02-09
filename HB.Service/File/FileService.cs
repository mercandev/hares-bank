using System;
using HB.Domain.Model;
using HB.Service.Const;
using HB.SharedObject;
using Marten;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Threading;
using PdfTurtleClientDotnet.Models;
using PdfTurtleClientDotnet;
using System.IO;

namespace HB.Service.File
{
    public class FileService : IFileService
    {
        private readonly IQuerySession _querySession;
        private readonly IPdfTurtleClient _pdfTurtleClient;

        public FileService(IQuerySession querySession, IPdfTurtleClient pdfTurtleClient)
        {
            this._querySession = querySession;
            this._pdfTurtleClient = pdfTurtleClient;
        }

        public async Task<ReturnState<object>> GenerateReceiptPdf(Guid transactionsId)
        {
            if (transactionsId == default(Guid)) throw new Exception("TransactionId can not be a empty!");

            var transactions = FindTransaction(transactionsId);

            if (transactions == null) throw new Exception("Transaction not found!");

            var receiptHtmlContent = ReceiptFormatWithTransactionsData(transactions);

            var renderData = new RenderData
            {
                Html = receiptHtmlContent,
                Options = new RenderOptions { PageFormat = RenderOptionsPageFormat.A3}
            };

            var pdfStream = await _pdfTurtleClient.RenderAsync(renderData);

            return new ReturnState<object>(pdfStream);
        }

        public async Task<ReturnState<object>> TransactionsHtml(Guid transactionsId)
        {
            if (transactionsId == default(Guid)) throw new Exception("TransactionId can not be a empty!");

            var transactions = FindTransaction(transactionsId);

            if (transactions == null) throw new Exception("Transaction not found!");

            var receiptHtmlContent = ReceiptFormatWithTransactionsData(transactions);

            return new ReturnState<object>(receiptHtmlContent);
        }

        private Transactions FindTransaction(Guid transactionsId)
        {
            return _querySession.Query<Transactions>().Where(x => x.Id == transactionsId).FirstOrDefault();
        }

        private string ReceiptFormatWithTransactionsData(Transactions transactions)
        {
           return ReceiptConst.RECEIPT_HTML
           .Replace("{0}", transactions.Id.ToString())
           .Replace("{1}", transactions.CreatedDate.ToString())
           .Replace("{2}", transactions.ReceiptInformation.SenderName)
           .Replace("{3}", transactions.ReceiptInformation.SenderIban)
           .Replace("{4}", transactions.ReceiptInformation.ReciverName)
           .Replace("{5}", transactions.ReceiptInformation.ReciverIban)
           .Replace("{6}", transactions.ReceiptInformation.TransactionExplanation)
           .Replace("{7}", transactions.ReceiptInformation.Balance.ToString())
           .Replace("\n", @"")
           .Replace("\"", @"'");
        }
    }
}

