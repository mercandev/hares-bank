using System;
using HB.Domain.Model;
using Marten;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HB.SharedObject;
using HB.SharedObject.CustomerViewModel;
using AutoMapper;
using System.Security.Claims;
using HB.Service.Engine;
using Microsoft.Extensions.Options;
using HB.Service.Helpers;
using HB.Infrastructure.Jwt;
using HB.Infrastructure.Exceptions;
using HB.SharedObject.AccountViewModel;
using System.ComponentModel;
using System.Reflection;
using HB.SharedObject.ExchangeViewModel;
using HB.Service.Const;
using HB.Service.Transaction;
using Microsoft.AspNetCore.SignalR;
using HB.Infrastructure.DbContext;
using HB.Infrastructure.Repository;
using HB.Infrastructure.Authentication;
using System.Net;

namespace HB.Service.Customer
{
	public class CustomerService : ICustomerService
	{
        #region Data Access

        private readonly HbContext? _hBContext;
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IOptions<JwtModel> _options;
        private readonly IRepository<Customers> _customerRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Accounts> _accountRepository;
        private readonly IAuthUserInformation _userInformation;

        #endregion

        #region Constructor

        public CustomerService(
            HbContext hbContext,
            IDocumentSession documentSession,
            IQuerySession querySession,
            IMapper mapper,
            ITransactionService transactionService,
            IOptions<JwtModel> options,
            IRepository<Customers> customerRepository,
            IRepository<Address> addressRepository,
            IRepository<Accounts> accountRepository,
            IAuthUserInformation userInformation
            )
        {
            this._hBContext = hbContext;
            this._documentSession = documentSession;
            this._querySession = querySession;
            this._mapper = mapper;
            this._transactionService = transactionService;
            this._options = options;
            this._customerRepository = customerRepository;
            this._userInformation = userInformation;
            this._addressRepository = addressRepository;
            this._accountRepository = accountRepository;
        }
        #endregion

        #region Convert Money Coal
        public async Task<ReturnState<object>> BuyGold(int customerId, ConvertMoneyToCoalViewModel model)
        {
            if (model.Price < 1M) throw new Exception("Price cannot under the 1.0!");

            var customerAccount = _hBContext.Accounts
                .Include("Customers")
                .Where(x => x.CustomersId == customerId).ToList();

            if (customerAccount == null) throw new Exception("Account not found!");

            var coalAccount = customerAccount.Where(x => x.CurrencyId == (int)Currency.GOLD).FirstOrDefault();

            if (coalAccount == null) throw new Exception("Coal account not found!");

            var mainAccount = customerAccount.Where(x => x.Id == model.AccountId).FirstOrDefault();

            var isAccountAmountEnough = mainAccount.Amount - model.Price;

            if (isAccountAmountEnough < 0M) throw new Exception("Balance not enough!");

            var goldAccount = customerAccount.Where(x => x.CurrencyId == (int)Currency.GOLD).FirstOrDefault();

            var goldAmount = model.Price / model.CoalPrice;

            var poundPrice = Math.Round(goldAmount, 5);

            mainAccount.Amount = mainAccount.Amount - model.Price;

            goldAccount.Amount = goldAccount.Amount + poundPrice;
            goldAccount.UpdatedDate = DateTime.Now;

            _hBContext.Entry(goldAccount).State = EntityState.Modified;
            _hBContext.Entry(mainAccount).State = EntityState.Modified;
            await _hBContext.SaveChangesAsync();

            var transactions = GoldBuyyingTransaction(goldAccount, poundPrice, model.Price);

            _transactionService.CreateTransaction(transactions);

            return new ReturnState<object>(true);
        }

        #endregion

        #region Coal Information

        public async Task<ReturnState<object>> CoalInformation(CoalDetailViewModel model)
        {
            var exchangeResult = await RestRequestHelper<ExchangeResponseViewModel>.GetService(ExchangeConst.EXCHANGE_URL);

            if (exchangeResult == null) throw new Exception("Exchange service return null!");

            if (model.CoalId == (int)Coals.Gold)
            {
                var exchangeGoldResult = exchangeResult.GA;

                var mapping = _mapper.Map<ExchangeMappingResponseViewModel>(exchangeGoldResult);

                return new ReturnState<object>(mapping);
            }

            if (model.CoalId == (int)Coals.Silver)
            {
                var exchangeSilverResult = exchangeResult.GAG;

                var mapping = _mapper.Map<ExchangeMappingResponseViewModel>(exchangeSilverResult);

                return new ReturnState<object>(mapping);
            }

            throw new HbBusinessException("Out-of-process!");
        }

        #endregion

        #region Create Account
        public async Task<ReturnState<object>> CreateAccount(int customerId, CreateAccountViewModel model)
        {
            if (model.CurrencyId < 0 || model.CurrencyId > 5) throw new Exception("CurrencyId is not valid!");

            var customerInformation = _hBContext?.Customers.Where(x => x.Id == customerId).FirstOrDefault();

            if (customerInformation == null) throw new Exception("Customer not found!");

            Accounts accounts = new()
            {
                BranchOfficesId = customerInformation.BranchOfficesId,
                CurrencyId = model.CurrencyId,
                Iban = GeneratorHelper.IbanGenerator(),
                CustomersId = customerInformation.Id,
                Name = $"{HbHelpers.GetEnumDescriptionIntToEnum((Currency)model.CurrencyId)} hesabım",
                CreatedBy = customerInformation.Name
            };

            await _hBContext.AddAsync(accounts);
            _hBContext.SaveChangesAsync();

            return new ReturnState<object>(true);
        }

        #endregion

        #region Create Customer
        public async Task<ReturnState<object>> CreateCustomer(CreateCustomerViewModel createCustomerViewModel)
        {
            var customerMappingModel = _mapper.Map<Customers>(createCustomerViewModel);

            var customer = await _customerRepository.AddAsync(customerMappingModel);

            createCustomerViewModel.CustomerId = customer.Id;

            var addressMappingModel = _mapper.Map<Address>(createCustomerViewModel);

            await _addressRepository.AddAsync(addressMappingModel);

            var accountMappingModel = _mapper.Map<Accounts>(createCustomerViewModel);

            await _accountRepository.AddAsync(accountMappingModel);

            return new ReturnState<object>(statusCode: HttpStatusCode.Created, data: true);
        }
        #endregion

        #region Customer Information
        public ReturnState<object> CustomerInformation()
        {
            var customerResult = _hBContext.Customers.Where(x => x.Id == _userInformation.CustomerId).FirstOrDefault();

            if (customerResult == null)
            {
                throw new Exception("Customer not found!");
            }

            var result = _hBContext?.Accounts.Where(x => x.Customers.Id == customerResult.Id)
               .Include("Customers")
               .Include("BranchOffices")
               .ToList();

            var address = _hBContext?.Address.Where(x => x.CustomerId == customerResult.Id).FirstOrDefault();

            var accountResult = _mapper.Map<List<AccountsViewModel>>(result);

            var customerInformationResult = new CustomerInformationViewModel
            {
                Name = customerResult.Name,
                Surname = customerResult.Surname,
                PhoneNumber = customerResult.PhoneNumber,
                Email = customerResult.Email,
                Branch = new BranchViewModel
                {
                    Id = customerResult.BranchOffices.Id,
                    Name = customerResult.BranchOffices.Name
                },
                Address = new AddressViewModel
                {
                    Id = address.Id,
                    AddressExplanation = address.AddressExplanation
                },
                Accounts = accountResult
            };

            return new ReturnState<object>(customerInformationResult);

        }
        #endregion

        #region Delegate Card Customer
        public async Task<ReturnState<object>> DelegateCardCustomer(int customerId , CardType cardType)
        {
            if (customerId == default) throw new HbBusinessException("CustomerId cannot be null or default!");

            var customerResult = _hBContext?.Accounts.Where(x => x.Customers.Id == customerId)
               .Include("Customers")
               .Include("BranchOffices")
               .FirstOrDefault();

            if (customerResult == null) throw new HbBusinessException("Customer not found!");

            var emptyRandomCard = _querySession.Query<Cards>()
                .Where(x => !x.IsActive && x.CustomerName == null).ToList().PickRandom();

            if (emptyRandomCard == null) throw new HbBusinessException("Blank card not found! Please produce new cards!");

            emptyRandomCard.CustomerId = customerResult.Customers.Id;
            emptyRandomCard.CustomerName = $"{customerResult.Customers.Name} {customerResult.Customers.Surname}";
            emptyRandomCard.CardType = cardType;
            emptyRandomCard.IsActive = true;
            if (cardType == CardType.DebitCard) emptyRandomCard.AccountId = customerResult.Id;

            _documentSession.Update(emptyRandomCard);
            await _documentSession.SaveChangesAsync();

            return new ReturnState<object>(true);
        }


        private Transactions GoldBuyyingTransaction(Accounts model, decimal goldPrice , decimal moneyPrice)
        {
            return new Transactions
            {
                AccountId = model.Id,
                CustomerId = model.CustomersId,
                ProccessType = ProccessType.InHouseTransaction,
                TransactionsType = TransactionsType.BuyyingCoal,
                Explanation = $"Altın Alım - Miktar: {goldPrice}",
                ReceiptInformation = new ReceiptInformation
                {
                    Balance = moneyPrice,
                    SenderIban = model.Iban,
                    SenderName = $"{model.Customers.Name} {model.Customers.Surname}",
                    ReciverName = PaymentIbanConst.HARES_BANK,
                    ReciverIban = PaymentIbanConst.HARES_BANK_IBAN,
                    TransactionExplanation = $"Hares Bank üzerinden altın alım. Miktar: {goldPrice}"
                }
            };
        }

        #endregion
    }
}



