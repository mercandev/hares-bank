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
using HB.Infrastructure.MartenRepository;
using System.Net;
using HB.Service.Helpers;

namespace HB.Service.File
{
    public class FileService : IFileService
    {
        private readonly IMartenRepository<Transactions> _martenRepository; 
        private readonly IPdfTurtleClient _pdfTurtleClient;

        public FileService(IMartenRepository<Transactions> martenRepository, IPdfTurtleClient pdfTurtleClient)
        {
            this._martenRepository = martenRepository;
            this._pdfTurtleClient = pdfTurtleClient;
        }

        public async Task<ReturnState<object>> GenerateReceiptPdf(Guid transactionsId)
        {
            if (transactionsId == default(Guid))
            {
                return new ReturnState<object>(HttpStatusCode.NotAcceptable, "TransactionId can not be a empty!");
            }

            var transactions = await FindTransaction(transactionsId);

            if (transactions is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotAcceptable, "Transaction not found!");
            }

            var receiptHtmlContent = FileHelper.ReceiptFormatWithTransactionsData(transactions);

            var renderData = new RenderData
            {
                Html = receiptHtmlContent,
                Options = new RenderOptions
                {
                    PageFormat = RenderOptionsPageFormat.A3
                }
            };

            var pdfStream = await _pdfTurtleClient.RenderAsync(renderData);

            return new ReturnState<object>(pdfStream);
        }

        public async Task<ReturnState<object>> TransactionsHtml(Guid transactionsId)
        {
            if (transactionsId == default(Guid))
            {
                return new ReturnState<object>(HttpStatusCode.NotAcceptable, "TransactionId can not be a empty!");
            }

            var transactions = await FindTransaction(transactionsId);

            if (transactions is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotAcceptable, "Transaction not found!");
            }

            var receiptHtmlContent = FileHelper.ReceiptFormatWithTransactionsData(transactions);

            return new ReturnState<object>(receiptHtmlContent);
        }

        private async Task<Transactions> FindTransaction(Guid transactionsId)
            => await _martenRepository.FirstOrDefaultAsync(x => x.Id == transactionsId);


    }
}

