using Application.Common;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;
using Polly;
using Square;
using Square.Authentication;
using Square.Exceptions;
using Square.Models;
using Stripe;
using Stripe.Forwarding;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace MTR_Fieldo_API.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly MtrContext _db;
        private readonly ResponseDto _responseDto;
        private Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private static string StripeApiKey;
        private static string SquareAccessKey;
        public static SquareClient client;
        private readonly IWalletService _walletService;
        private static string bucketName;
        private readonly ICommonService _commonService;


        public PaymentService(MtrContext db, Microsoft.Extensions.Configuration.IConfiguration configuration, IWalletService walletService, ICommonService commonService)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _configuration = configuration;
            StripeApiKey = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("Stripe:DevApiKey").Value.ToString() : _configuration.GetSection("Stripe:LiveApiKey").Value.ToString());
            SquareAccessKey = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("Square:DevAccessToken").Value.ToString() : _configuration.GetSection("Square:LiveAccessToken").Value.ToString());
            client = new SquareClient.Builder()
    .BearerAuthCredentials(
         new BearerAuthModel.Builder(
             SquareAccessKey
         )
         .Build())
    .Environment(Square.Environment.Sandbox)
    .Build();
            _walletService = walletService;
            bucketName = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:DEVBucketName").Value.ToString() : _configuration.GetSection("AWS:BucketName").Value.ToString());
            _commonService = commonService;
        }
        public async Task<ResponseDto> CreatePaymentIntent(Fieldo_UserDetails user,string stripeCustomerId, PaymentDto payment)
        {
            try
            {
             //   StripeConfiguration.ApiKey = "sk_test_51P8GHRSBJd9qPRs4hLSo6YZ4uF6F2RmTlYlmMa93CmZdK8LL1t1qm2oXTy7kGtzWTspxdX4NhTOCCSuShfFvjrWM00ssScEtyb";
                StripeConfiguration.ApiKey = StripeApiKey;

                var options = new PaymentIntentCreateOptions
                {
                    Amount = payment.Amount,
                    Currency = payment.currency,
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    },
                    Metadata = new Dictionary<string, string> { { "requestId", Guid.NewGuid().ToString() },{ "userId", user.Id.ToString() } },
                    Customer= stripeCustomerId,
                    SetupFutureUsage= "on_session",
                    Description="New transaction"
                };
                var service = new PaymentIntentService();
                var paymentIntent = service.Create(options);


                var ephemeralKeyParams = new EphemeralKeyCreateOptions();
                ephemeralKeyParams.StripeVersion = "2022-08-01";
                ephemeralKeyParams.Customer = paymentIntent.CustomerId;
                var ephemeralKeyService = new EphemeralKeyService();
                var ephemeralKey = ephemeralKeyService.Create(ephemeralKeyParams);
               // Fieldo_Payments stripePayments = new Fieldo_Payments()
               // {
               //     Amount = paymentIntent.Amount,
               //     UserId=userId,
               //     CustomerId = paymentIntent.CustomerId,
               //     Currency = paymentIntent.Currency,
               //     Description = paymentIntent.Description,
               //     Status = paymentIntent.Status,
               //     CreatedAt = DateTime.Now,
               //     IntentId= paymentIntent.Id,
                  
               // };

               //await _db.Fieldo_Payments.AddAsync(stripePayments);
               // await _db.SaveChangesAsync();
                PaymentIntentsResponse paymentIntentsResponse = new PaymentIntentsResponse
                {
                    IntentId= paymentIntent.Id,
                    customer = user.StripeCustomerId,
                    ephemeralKey = ephemeralKey.Secret,
                    paymentIntent = paymentIntent.ClientSecret,
                    publishableKey = "pk_test_jSJENouMrA6mvNa74Z47txKs"
                };
                _responseDto.Result = paymentIntentsResponse;
                _responseDto.IsSuccess = true;
               
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error occured";
            }
            return _responseDto;


        }
      

         public async Task<ResponseDto> UpdatePaymentStatus(PaymentstatusDto payment)
        {
            try
            {
                var service = new PaymentIntentService();
                if (payment.PaymentMethod== Application.Common.PaymentMethod.Stripe)
                {
                    StripeConfiguration.ApiKey = StripeApiKey;
                    var paymentIntent =  service.Get(payment.Intent);
                    //var paymentIntent = _db.Fieldo_Payments.Where(x => x.IntentId == intent.Id).FirstOrDefault();
                    //if (paymentIntent != null)
                    //{
                    //    paymentIntent.Status = intent.Status;
                    //}

                    //_db.Fieldo_Payments.Update(paymentIntent);
                    string userId;
                    string requestId;
                    paymentIntent.Metadata.TryGetValue("userId", out userId);
                    paymentIntent.Metadata.TryGetValue("requestId", out requestId);
                    //paymentIntent.Amount = paymentIntent.AmountReceived / 100;
                    var transaction = _db.Fieldo_Payments.Where(x => x.RequestId == requestId).FirstOrDefault();
                    if (transaction==null)
                    {
                        Fieldo_Payments stripePayments = new Fieldo_Payments()
                        {
                            Amount = Convert.ToDouble(paymentIntent.AmountReceived) / 100,
                            UserId = Convert.ToInt32(userId),
                            CustomerId = paymentIntent.CustomerId,
                            Currency = paymentIntent.Currency,
                            Description = paymentIntent.Description,
                            Status = paymentIntent.Status,
                            CreatedAt = DateTime.Now,
                            IntentId = paymentIntent.Id,
                            RequestId = requestId

                        };
                        await _db.Fieldo_Payments.AddAsync(stripePayments);
                        _db.SaveChanges();
                    }
                    else
                    {
                       
                        transaction.Amount =Convert.ToDouble (paymentIntent.AmountReceived) / 100;
                        transaction.UserId= Convert.ToInt32(userId);
                        transaction.CustomerId = paymentIntent.CustomerId;
                        transaction.Currency = paymentIntent.Currency;
                        transaction.Description = paymentIntent.Description;
                        transaction.Status = paymentIntent.Status;
                        transaction.UpdatedAt = DateTime.Now;
                        transaction.IntentId = paymentIntent.Id;
                        _db.Fieldo_Payments.Update(transaction);
                        _db.SaveChanges();
                    }

                  

                    if (paymentIntent.Status== StripePaymentStatus.succeeded.ToString())
                    {
                       if(! _db.Fieldo_Wallet.Where(x=>x.RequestId== requestId).Any())
                        {
                            
                            _walletService.AddAmountAsync(userId, Convert.ToDouble(paymentIntent.AmountReceived) / 100, paymentIntent.Currency, requestId);

                        }
                    }

                    _responseDto.Result = paymentIntent;
                    _responseDto.IsSuccess = true;

                }

                else
                {
                    var result = await client.PaymentsApi.GetPaymentAsync(paymentId: payment.Intent);
                    Fieldo_Payments squarePayments = new Fieldo_Payments()
                    {
                        Amount = result.Payment.AmountMoney.Amount,
                        CustomerId = result.Payment.CustomerId,
                        UserId = Convert.ToInt32( result.Payment.ReferenceId),
                        Currency = result.Payment.AmountMoney.Currency,
                        Description = "Amount Added in wallet",
                        Status = result.Payment.Status,
                        CreatedAt = DateTime.Now,
                        SquareId = result.Payment.Id,
                        IntentId= result.Payment.Id

                    };
                    _db.Fieldo_Payments.Add(squarePayments);
                    _db.SaveChanges();

                    if (result.Payment.Status == SquarePaymentStatus.Completed.ToString())
                    {
                        if (!_db.Fieldo_Wallet.Where(x => x.RequestId == result.Payment.Id).Any())
                        {
                            long amount = result.Payment.AmountMoney.Amount.Value;
                            _walletService.AddAmountAsync(result.Payment.ReferenceId, amount, result.Payment.AmountMoney.Currency, result.Payment.Id);

                        }
                    }
                    _responseDto.Result = result.Payment;
                    _responseDto.IsSuccess = true;

                }
               

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error occured";
            }
            return _responseDto;


        }
        public async Task<ResponseDto> DetachPaymentMethod(string paymentId)
        {
            try
            {
                StripeConfiguration.ApiKey = StripeApiKey;

                var service = new PaymentMethodService();
               var result= service.Detach("pm_1MqLiJLkdIwHu7ixUEgbFdYF"); ;
                _responseDto.Result = result;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;

        }

        public async Task<ResponseDto> CreateStripePaymentMethod(Fieldo_UserDetails fieldo_User, CardDto card)
        {
            try
            {
                StripeConfiguration.ApiKey = StripeApiKey;
                var options = new PaymentMethodCreateOptions
                {
                    Type = "card",
                    Card = new PaymentMethodCardOptions
                    {
                        Number = card.CardNumber,
                        ExpMonth = card.ExpMonth,
                        ExpYear = card.ExpYear,
                        Cvc = card.Cvc,
                    },
                };
                var service = new PaymentMethodService();
               var result= service.Create(options);
                _responseDto.Result = result;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;

        }
        public async Task<ResponseDto> AttachPaymentMethod(Fieldo_UserDetails fieldo_User, string paymentId)
        {
            try
            {
                StripeConfiguration.ApiKey = StripeApiKey;
                var options = new PaymentMethodAttachOptions { Customer = fieldo_User.StripeCustomerId };
                var service = new PaymentMethodService();
                var result = service.Attach(paymentId, options);
                _responseDto.Result = result;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;

        }

        public async Task<ResponseDto> CreateStripeCustomer(string Name,string Email)
        {
            try
            {
                StripeConfiguration.ApiKey = StripeApiKey;
                var options = new CustomerCreateOptions
                {
                    Name = Name,
                    Email = Email,
                };
                var service = new CustomerService();
                var customer= service.Create(options);

                _responseDto.Result = customer.Id;
                _responseDto.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;


        }

        public async Task<ResponseDto> CreateSquareCustomer(SquareCustomerDto createCustomer)
        {
            try
            {
                var body = new CreateCustomerRequest.Builder()
.IdempotencyKey(Guid.NewGuid().ToString())
.GivenName(createCustomer.Name)
//.FamilyName(createCustomer.FamilyName)
//.CompanyName(createCustomer.CompanyName)
//.Nickname(createCustomer.Nickname)
.EmailAddress(createCustomer.EmailAddress)
//.PhoneNumber(createCustomer.PhoneNumber)
//.ReferenceId(createCustomer.ReferenceId)
//.Note(createCustomer.Note)
//.Birthday(createCustomer.Birthday)
//.TaxIds(createCustomer.TaxIds)
.Build();
                var result = await client.CustomersApi.CreateCustomerAsync(body: body);
                _responseDto.Result = result.Customer.Id;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> CreateSquarePayment(PaymentRequestDto PaymentRequestDto, Fieldo_UserDetails UserDetail)
        {
            var amountMoney = new Money.Builder()
   .Amount(10L)
   .Currency("USD")
   .Build();

            //var tipMoney = new Money.Builder()
            //  .Amount(PaymentRequestDto.Money.Amount)
            //  .Currency(PaymentRequestDto.Money.Currency)
            //  .Build();

            //var appFeeMoney = new Money.Builder()
            //  .Amount(PaymentRequestDto.AppFee.Amount)
            //  .Currency(PaymentRequestDto.AppFee.Currency)
            //  .Build();

            //var billingAddress = new Square.Models.Address.Builder()
            //  .AddressLine1(PaymentRequestDto.Billing.AddressLine1)
            //  .AddressLine2(PaymentRequestDto.Billing.AddressLine2)
            //  .AddressLine3(PaymentRequestDto.Billing.AddressLine3)
            //  .Locality(PaymentRequestDto.Billing.Locality)
            //  .Sublocality(PaymentRequestDto.Billing.Sublocality)
            //  .Sublocality2(PaymentRequestDto.Billing.Sublocality2)
            //  .Sublocality3(PaymentRequestDto.Billing.Sublocality3)
            //  .AdministrativeDistrictLevel1(PaymentRequestDto.Billing.AdministrativeDistrictLevel1)
            //  .AdministrativeDistrictLevel2(PaymentRequestDto.Billing.AdministrativeDistrictLevel2)
            //  .AdministrativeDistrictLevel3(PaymentRequestDto.Billing.AdministrativeDistrictLevel3)
            //  .PostalCode(PaymentRequestDto.Billing.PostalCode)
            //  .Country(PaymentRequestDto.Billing.Country)
            //  .FirstName(PaymentRequestDto.Billing.FirstName)
            //  .LastName(PaymentRequestDto.Billing.LastName)
            //  .Build();

            //var buyerSuppliedMoney = new Money.Builder()
            //  .Amount(PaymentRequestDto.BuyerSupplied.Amount)
            //  .Currency(PaymentRequestDto.BuyerSupplied.Currency)
            //  .Build();

            //var changeBackMoney = new Money.Builder()
            //  .Amount(PaymentRequestDto.ChangeBack.Amount)
            //  .Currency(PaymentRequestDto.ChangeBack.Currency)
            //  .Build();

            //var cashDetails = new CashPaymentDetails.Builder(buyerSuppliedMoney: buyerSuppliedMoney)
            //  .ChangeBackMoney(changeBackMoney)
            //  .Build();

            //var externalDetails = new ExternalPaymentDetails.Builder(type: PaymentRequestDto.ExternalPayment.Type, source: PaymentRequestDto.ExternalPayment.Source)
            //  .SourceId("ewe")
             // .Build();

            var body = new CreatePaymentRequest.Builder(sourceId: "cnon:card-nonce-ok", idempotencyKey: "6359e429-b388-4b82-b0ad-ee4fd17ab283")
              .AmountMoney(amountMoney)
             // .TipMoney(tipMoney)
              //.AppFeeMoney(appFeeMoney)
              //.DelayDuration(PaymentRequestDto.PaymentRequest.DelayDuration)
              //.DelayAction(PaymentRequestDto.PaymentRequest.DelayAction)
              //.OrderId(PaymentRequestDto.PaymentRequest.OrderId)
              .CustomerId(UserDetail.SquareCustomerId)
              //.LocationId(PaymentRequestDto.PaymentRequest.LocationId)
              //.TeamMemberId(PaymentRequestDto.PaymentRequest.TeamMemberId)
              .ReferenceId(UserDetail.Id.ToString())
              //.VerificationToken(PaymentRequestDto.PaymentRequest.VerificationToken)
              //.AcceptPartialAuthorization(PaymentRequestDto.PaymentRequest.AcceptPartialAuthorization)
              //.BuyerEmailAddress(PaymentRequestDto.PaymentRequest.BuyerEmailAddress)
              //.BillingAddress(billingAddress)
              //.Note(PaymentRequestDto.PaymentRequest.Note)
              //.StatementDescriptionIdentifier(PaymentRequestDto.PaymentRequest.StatementDescriptionIdentifier)
              //.CashDetails(cashDetails)
              //.ExternalDetails(externalDetails)
              .Build();

            try
            {
                var result = await client.PaymentsApi.CreatePaymentAsync(body: body);
                Fieldo_Payments squarePayments = new Fieldo_Payments()
                {
                    Amount = result.Payment.AmountMoney.Amount,
                    CustomerId = result.Payment.CustomerId,
                    UserId= UserDetail.Id,
                    Currency = result.Payment.AmountMoney.Currency,
                    Description = "Amount Added in wallet",
                    Status = result.Payment.Status,
                    CreatedAt = DateTime.Now,
                    SquareId = result.Payment.Id
                      
                };

                 _db.Fieldo_Payments.Add(squarePayments);
                 _db.SaveChanges();
                _responseDto.Result = result.Payment;
                _responseDto.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> UpdateSquarePayment(UpdatePaymentDto updatePaymentDto)
        {
            var amountMoney = new Money.Builder()
  .Amount(updatePaymentDto.Money.Amount)
  .Currency(updatePaymentDto.Money.Currency)
  .Build();

            var tipMoney = new Money.Builder()
              .Amount(updatePaymentDto.Tip.Amount)
              .Currency(updatePaymentDto.Tip.Currency)
              .Build();

            var appFeeMoney = new Money.Builder()
              .Amount(updatePaymentDto.AppFee.Amount)
              .Currency(updatePaymentDto.AppFee.Currency)
              .Build();

            var approvedMoney = new Money.Builder()
              .Amount(updatePaymentDto.Approved.Amount)
              .Currency(updatePaymentDto.Approved.Currency)
              .Build();

            var buyerSuppliedMoney = new Money.Builder()
              .Amount(updatePaymentDto.BuyerSupplied.Amount)
              .Currency(updatePaymentDto.BuyerSupplied.Currency)
              .Build();

            var changeBackMoney = new Money.Builder()
              .Amount(updatePaymentDto.ChangeBack.Amount)
              .Currency(updatePaymentDto.ChangeBack.Currency)
              .Build();

            var cashDetails = new CashPaymentDetails.Builder(buyerSuppliedMoney: buyerSuppliedMoney)
              .ChangeBackMoney(changeBackMoney)
              .Build();

            var payment = new Payment.Builder()
              .AmountMoney(amountMoney)
              .TipMoney(tipMoney)
              .AppFeeMoney(appFeeMoney)
              .ApprovedMoney(approvedMoney)
              .DelayAction(updatePaymentDto.Payment.DelayAction)
              .CashDetails(cashDetails)
              .VersionToken(updatePaymentDto.Payment.VersionToken)
              .Build();

            var body = new UpdatePaymentRequest.Builder(idempotencyKey: null)
              .Payment(payment)
              .Build();

            try
            {
                var result = await client.PaymentsApi.UpdatePaymentAsync(paymentId: null, body: body);
                _responseDto.Result = result.Payment;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }
        public async Task<ResponseDto> CancelSquarePayment(string PaymentId)
        {
            try
            {
                var result = await client.PaymentsApi.CancelPaymentAsync(paymentId: PaymentId);
                _responseDto.Result = result;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }
        public async Task<ResponseDto> AddSquareCard(Fieldo_UserDetails fieldo_User, SquareCardDto squareCardDto)
        {
            var billingAddress = new Square.Models.Address.Builder()
   .AddressLine1(squareCardDto.Address.AddressLine1)
   .AddressLine2(squareCardDto.Address.AddressLine2)
   .AddressLine3(squareCardDto.Address.AddressLine3)
   .Locality(squareCardDto.Address.Locality)
   .Sublocality(squareCardDto.Address.Sublocality)
   .Sublocality2(squareCardDto.Address.Sublocality2)
   .Sublocality3(squareCardDto.Address.Sublocality3)
   .AdministrativeDistrictLevel1(squareCardDto.Address.AdministrativeDistrictLevel1)
   .AdministrativeDistrictLevel2(squareCardDto.Address.AdministrativeDistrictLevel2)
   .AdministrativeDistrictLevel3(squareCardDto.Address.AdministrativeDistrictLevel3)
   .PostalCode(squareCardDto.Address.PostalCode)
   .Country(squareCardDto.Address.Country)
   .FirstName(squareCardDto.Address.FirstName)
   .LastName(squareCardDto.Address.LastName)
   .Build();

            var card = new Square.Models.Card.Builder()
              .ExpMonth(squareCardDto.CardDetails.ExpMonth)
              .ExpYear(squareCardDto.CardDetails.ExpYear)
              .CardholderName(squareCardDto.CardDetails.CardholderName)
              .BillingAddress(billingAddress)
              .CustomerId(squareCardDto.CardDetails.CustomerId)
              .ReferenceId(squareCardDto.CardDetails.ReferenceId)
              .Version(squareCardDto.CardDetails.Version)
              .Build();

            var body = new CreateCardRequest.Builder(
                idempotencyKey:Guid.NewGuid().ToString(),
                sourceId: "as",
                card: card)
              .VerificationToken("as")
              .Build();

            try
            {
                var result = await client.CardsApi.CreateCardAsync(body: body);
                _responseDto.Result = result;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> DisableSquareCard(Fieldo_UserDetails fieldo_User, string cardId)
        {
            try
            {
                var result = await client.CardsApi.DisableCardAsync(cardId: cardId);
                _responseDto.Result = result;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> CompleteSquarePayment(Fieldo_UserDetails fieldo_User, string paymentId, string VersionToken)
        {
            var body = new CompletePaymentRequest.Builder()
   .VersionToken(VersionToken)
   .Build();

            try
            {
                var result = await client.PaymentsApi.CompletePaymentAsync(paymentId: paymentId, body: body);
                _responseDto.Result = result;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetMyTransactionHistory(Fieldo_UserDetails fieldo_User, int domainId)
        {
            try
            {
                List<PaymentHistoryViewModel> PaymentHistory = new List<PaymentHistoryViewModel>();

                var tasks = await _db.Fieldo_Task
                                          .Include(u => u.UserDetailsCreatedBy)
                                          .Include(u => u.UserDetailsAssignedTo)
                                           .Include(u => u.UserDetailsViewedBy)
                                          .Include(u => u.UserDetailsUpdatedBy)
                                          .Include(u => u.UserDetailsAssingedBy)
                                           .Include(u => u.TaskCategory)
                                          .Where(t => t.CreatedBy == fieldo_User.Id && t.DomainId == domainId)
                                          .ToListAsync();
                //var Transactions= _db.Fieldo_Payments.
                //    Include(x => x.Task.UserDetailsAssignedTo).
                //       Include(x => x.Task.TaskCategory)
                //    .Include(x => x.Task).Select(x => new
                //    {
                //     Id=   x.Id,
                //     TaskCategory=  x.Task.TaskCategory.Name,
                //     ServiceProvider = x.Task.UserDetailsAssignedTo.FirstName+ " "+ x.Task.UserDetailsAssignedTo.LastName,
                //    Amount = x.Amount,
                //        CreatedAt = x.CreatedAt,
                //        Status= x.Status
                //    }
                //   )
                //    .
                //    ToList();

                foreach (var item in tasks)
                {


                    PaymentHistory.Add(new PaymentHistoryViewModel
                    {
                        Id = item.Id,
                        ServiceType = item.TaskCategory.Name,
                        ServiceProvider = item.UserDetailsAssignedTo != null ? item.UserDetailsAssignedTo.FirstName : "Not Assigned Yet", // If UserDetailsAssignedTo is null, assign an empty string
                        Amount = item.Amount ?? 0, // If Amount is null, assign 0
                        CreatedAt = item.CreatedAt,
                        PaymentDateTime= item.PaymentDateTime,
                        Status = item.PaymentStatus ?? "" ,// If PaymentStatus is null, assign an empty string
                        ServiceProviderOrCategoryIcon= item.UserDetailsAssignedTo != null &&
                                !string.IsNullOrEmpty(item.UserDetailsAssignedTo.ProfileUrl)
                                ? _commonService.GeneratePreSignedURL(bucketName, item.UserDetailsAssignedTo.ProfileUrl, 7200)
                                : ""
                    });
                }



                //{
                //    new PaymentHistoryViewModel { Id = 1, ServiceType = "Cleaning", ServiceProvider = "Provider A", Amount = 100.0m, CreatedAt = DateTime.Now, Status = "Completed" },
                //    new PaymentHistoryViewModel { Id = 2, ServiceType = "Repair", ServiceProvider = "Provider B", Amount = 200.0m, CreatedAt = DateTime.Now.AddDays(-1), Status = "Pending" },
                //    new PaymentHistoryViewModel{ Id = 3, ServiceType = "Maintenance", ServiceProvider = "Provider C", Amount = 150.0m, CreatedAt = DateTime.Now.AddDays(-2), Status = "In Progress" }
                //};

                _responseDto.Result = PaymentHistory;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;



        }
        async Task<List<ITransaction>> GetStripeTransactionsAsync(Fieldo_UserDetails fieldo_User)
        {
            var stripeTransactions = await _db.Fieldo_Payments
                .Where(x => x.CustomerId == fieldo_User.StripeCustomerId)
                .ToListAsync();

            return stripeTransactions.Cast<ITransaction>().ToList();
        }
        async Task<List<ITransaction>> GetSquareTransactionsAsync(Fieldo_UserDetails fieldo_User)
        {
            var squareTransactions = await _db.Fieldo_Payments
                .Where(x => x.CustomerId == fieldo_User.SquareCustomerId)
                .ToListAsync();

            return squareTransactions.Cast<ITransaction>().ToList();
        }
        async Task<List<ITransaction>> MergeTransactionsAsync(List<ITransaction> stripeTransactions, List<ITransaction> squareTransactions)
        {
            var combinedTransactions = new List<ITransaction>();
            combinedTransactions.AddRange(stripeTransactions);
            combinedTransactions.AddRange(squareTransactions);
            return combinedTransactions;
        }

        public async Task<ResponseDto> GetMyMonthlySpent(Fieldo_UserDetails fieldo_User)
        {

            try
            {
                // Calculate the date 30 days ago from today
                var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

                // Fetch all successful transactions for the customer in the last 30 days and sum the amount
                var totalAmountLast30Days = await _db.Fieldo_Payments
                    .Where(x => x.UserId == fieldo_User.Id && x.Status == "success" && x.CreatedAt >= thirtyDaysAgo)
                    .SumAsync(x => x.Amount ?? 0);
                _responseDto.Result = totalAmountLast30Days;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
       
        }

        public async Task<ResponseDto> GetMyTotalSpentByMonthAsync(Fieldo_UserDetails fieldo_User, int year, int month)
        {

            try
            {
                // Calculate the start and end dates of the specified month
                var firstDayOfMonth = new DateTime(year, month, 1);
                var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

                // Fetch all successful transactions for the customer in the specified month and sum the amount
                var totalAmountForMonth = await _db.Fieldo_Payments
                    .Where(x => x.UserId == fieldo_User.Id && x.Status == "success" && x.CreatedAt >= firstDayOfMonth && x.CreatedAt < firstDayOfNextMonth)
                    .SumAsync(x => x.Amount ?? 0);
                _responseDto.Result = totalAmountForMonth;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;


        }

        public async Task<ResponseDto> GetMyTotalSpent(Fieldo_UserDetails fieldo_User)
        {

            try
            {
                var totalAmount = await _db.Fieldo_Payments
    .Where(x => x.UserId == fieldo_User.Id && x.Status == "success")
    .SumAsync(x => x.Amount ?? 0);
                _responseDto.Result = totalAmount;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
           
           
        }
        public async Task<ResponseDto> GetMyEarning(Fieldo_UserDetails fieldo_User)
        {
            try
            {
                // Calculate the date 30 days ago from today
                var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

                // Fetch all tasks assigned to the specified user
                var assignedTasks = await _db.Fieldo_Task
                    .Where(t => t.AssignedTo == fieldo_User.Id)
                    .ToListAsync();

                // Fetch all successful tasks for the last 30 days and sum the amount
                var totalEarningsLast30Days = assignedTasks
                    .Where(t => t.PaymentStatus == "success" && t.CreatedAt >= thirtyDaysAgo)
                    .Sum(t => t.Amount ?? 0);

                // Fetch all successful tasks for all time and sum the amount
                var totalEarningsAllTime = assignedTasks
                    .Where(t => t.PaymentStatus == "success")
                    .Sum(t => t.Amount ?? 0);

                // Return the total earnings for the last 30 days and for all time
                var earnings = new
                {
                    TotalEarningsLast30Days = totalEarningsLast30Days,
                    TotalEarningsAllTime = totalEarningsAllTime
                };

                _responseDto.Result = earnings;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException?.Message}";
            }
            return _responseDto;
        }

        //public async Task<ResponseDto> GetMyEarning(Fieldo_UserDetails fieldo_User)
        //{
        //    try
        //    {
        //        // Calculate the date 30 days ago from today
        //        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

        //        // Fetch all tasks assigned to the specified user
        //        var assignedTasks = await _db.Fieldo_Task
        //            .Where(t => t.AssignedTo == fieldo_User.Id)
        //            .Select(t => t.Id)
        //            .ToListAsync();

        //        // Fetch all successful payments for these tasks in the last 30 days and sum the amount
        //        var totalEarningsLast30Days = await _db.Fieldo_Payments
        //            .Where(p => assignedTasks.Contains(p.TaskId) && p.Status == "success" && p.CreatedAt >= thirtyDaysAgo)
        //            .SumAsync(p => p.Amount ?? 0);

        //        // Fetch all successful payments for these tasks for all time and sum the amount
        //        var totalEarningsAllTime = await _db.Fieldo_Payments
        //            .Where(p => assignedTasks.Contains(p.TaskId) && p.Status == "success")
        //            .SumAsync(p => p.Amount ?? 0);

        //        // Return the total earnings for the last 30 days and for all time
        //       var earning=   new
        //        {
        //            TotalEarningsLast30Days = totalEarningsLast30Days,
        //            TotalEarningsAllTime = totalEarningsAllTime
        //        };

        //        _responseDto.Result = earning;
        //        _responseDto.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _responseDto.IsSuccess = false;
        //        _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
        //    }
        //    return _responseDto;
        //}

        public async Task<object> GetMyEarningByUserId(int userId)
        {
            // Calculate the date 30 days ago from today
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            // Fetch all tasks assigned to the specified user
            var assignedTasks = await _db.Fieldo_Task
                .Where(t => t.AssignedTo == userId)
                .ToListAsync();

            // Fetch all successful tasks for the last 30 days and sum the amount
            var totalEarningsLast30Days = assignedTasks
                .Where(t => t.PaymentStatus == "success" && t.CreatedAt >= thirtyDaysAgo)
                .Sum(t => t.Amount ?? 0);

            // Fetch all successful tasks for all time and sum the amount
            var totalEarningsAllTime = assignedTasks
                .Where(t => t.PaymentStatus == "success")
                .Sum(t => t.Amount ?? 0);

            // Return the total earnings for the last 30 days and for all time
            var earning = new
            {
                TotalEarningsLast30Days = totalEarningsLast30Days,
                TotalEarningsAllTime = totalEarningsAllTime
            };

            return earning;
        }

        //public async Task<object> GetMyEarningByUserId(int userId)
        //{

        //        // Calculate the date 30 days ago from today
        //        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

        //        // Fetch all tasks assigned to the specified user
        //        var assignedTasks = await _db.Fieldo_Task
        //            .Where(t => t.AssignedTo == userId)
        //            .Select(t => t.Id)
        //            .ToListAsync();

        //        // Fetch all successful payments for these tasks in the last 30 days and sum the amount
        //        var totalEarningsLast30Days = await _db.Fieldo_Payments
        //            .Where(p => assignedTasks.Contains(p.TaskId) && p.Status == "success" && p.CreatedAt >= thirtyDaysAgo)
        //            .SumAsync(p => p.Amount ?? 0);

        //        // Fetch all successful payments for these tasks for all time and sum the amount
        //        var totalEarningsAllTime = await _db.Fieldo_Payments
        //            .Where(p => assignedTasks.Contains(p.TaskId) && p.Status == "success")
        //            .SumAsync(p => p.Amount ?? 0);

        //        // Return the total earnings for the last 30 days and for all time
        //        var earning = new
        //        {
        //            TotalEarningsLast30Days = totalEarningsLast30Days,
        //            TotalEarningsAllTime = totalEarningsAllTime
        //        };
        //    return earning;
        //}

        public interface ITransaction
        {
            // Define common properties or methods if applicable
        }
    }
}
