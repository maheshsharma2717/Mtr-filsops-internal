using Application.Common;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using System.Runtime.InteropServices.ComTypes;

namespace MTR_Fieldo_API.Service
{
    public class WalletService:IWalletService
    {
        private readonly MtrContext _context;

        public WalletService(MtrContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto> ChargeAmountAsync(string customerId, long amount, string currency, int? taskId)
        {
            var response = new ResponseDto();
            try
            {
                var wallet = await _context.Fieldo_Wallet.FirstOrDefaultAsync(w => w.CustomerId == customerId && w.IsActive && !w.IsDeleted && w.Currency == currency.ToLower());
                if (wallet == null)
                {
                    wallet = new Fieldo_Wallet
                    {
                        CustomerId = customerId,
                        Currency = currency.ToLower(),
                        Balance = amount,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        RequestId = Guid.NewGuid().ToString()
                    };
                    _context.Fieldo_Wallet.Add(wallet);
                    _context.SaveChanges();
                    response.IsSuccess = false;
                    response.Message = "Please add amount to wallet";
                    return response;
                }

                if (wallet.Balance < amount)
                {
                    response.IsSuccess = false;
                    response.Message = "Insufficient balance";
                    return response;
                }

                wallet.Balance -= amount;
                wallet.UpdatedAt = DateTime.UtcNow;
                _context.Fieldo_Wallet.Update(wallet);
               if(  await _context.SaveChangesAsync() > 0)
                {
                    if (taskId != null)
                    {
                       var task= await _context.Fieldo_Task.Where(x => x.Id == taskId).FirstOrDefaultAsync();
                        task.PaymentDateTime = DateTime.Now;
                        task.PaymentStatus = Application.Common.PaymentStatus.Completed.ToString() ;
                        task.Amount = amount;
                        _context.Update(task);
                        await _context.SaveChangesAsync();

                        Fieldo_WalletTransaction _WalletTransaction = new Fieldo_WalletTransaction()
                        {
                            Currency = currency.ToLower(),
                            Amount = amount,
                            CreatedAt = DateTime.UtcNow,
                            TransactionType = Application.Common.TransactionType.Debit.ToString(),
                            WalletId = wallet.Id,
                            UserId = Convert.ToInt32(customerId),
                            Description = "Added amount to wallet",
                             TaskId = taskId



                        };
                        _context.Fieldo_WalletTransaction.Add(_WalletTransaction);
                        _context.SaveChanges();

                    }
                    else
                    {
                        Fieldo_WalletTransaction _WalletTransaction = new Fieldo_WalletTransaction()
                        {
                            Currency = currency.ToLower(),
                            Amount = amount,
                            CreatedAt = DateTime.UtcNow,
                            TransactionType = Application.Common.TransactionType.Debit.ToString(),
                            WalletId = wallet.Id,
                            UserId = Convert.ToInt32(customerId),
                            Description = "Added amount to wallet",



                        };
                        _context.Fieldo_WalletTransaction.Add(_WalletTransaction);
                        _context.SaveChanges();

                    }
                };

                response.IsSuccess = true;
                response.Result = wallet.Balance;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"{ex.Message}\n {ex.InnerException?.Message}";
            }
            return response;
        }

        public async Task<ResponseDto> GetWalletBalanceAsync(string customerId, string currency)
        {
            var response = new ResponseDto();
            try
            {
                var wallet = await _context.Fieldo_Wallet.FirstOrDefaultAsync(w => w.CustomerId == customerId && w.IsActive && !w.IsDeleted && w.Currency == currency.ToLower());
                if (wallet == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Wallet not found or currency mismatch";
                    return response;
                }

                response.IsSuccess = true;
                response.Result = wallet.Balance;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"{ex.Message}\n {ex.InnerException?.Message}";
            }
            return response;
        }

       
        public async Task<ResponseDto> AddAmountAsync(string customerId, double amount, string currency, string requestId)
        {
            var response = new ResponseDto();
            try
            {
                var wallet =  _context.Fieldo_Wallet.FirstOrDefault(w => w.CustomerId == customerId && w.IsActive && !w.IsDeleted && w.Currency == currency);

                if (wallet == null)
                {
                    wallet = new Fieldo_Wallet
                    {
                        CustomerId = customerId,
                        Currency = currency.ToLower(),
                        Balance = amount,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        RequestId= requestId
                    };
                    _context.Fieldo_Wallet.Add(wallet);
                    _context.SaveChanges();
                    Fieldo_WalletTransaction _WalletTransaction = new Fieldo_WalletTransaction()
                    {
                        Currency = currency.ToLower(),
                        Amount = amount,
                        CreatedAt = DateTime.UtcNow,
                        TransactionType = Application.Common.TransactionType.Credit.ToString(),
                        WalletId = wallet.Id,
                        UserId = Convert.ToInt32(customerId),
                        Description = "Added amount to wallet",



                    };

                    _context.Fieldo_WalletTransaction.Add(_WalletTransaction);
                    _context.SaveChanges();


                }
                else
                {
                    wallet.Balance += amount;
                    wallet.UpdatedAt = DateTime.UtcNow;
                    _context.Fieldo_Wallet.Update(wallet);
                    _context.SaveChanges();
                    Fieldo_WalletTransaction _WalletTransaction = new Fieldo_WalletTransaction()
                    {
                        Currency = currency.ToLower(),
                        Amount = amount,
                        CreatedAt = DateTime.UtcNow,
                        TransactionType = Application.Common.TransactionType.Credit.ToString(),
                        WalletId = wallet.Id,
                        UserId = Convert.ToInt32(customerId),
                        Description = "Added amount to wallet",



                    };
                    _context.Fieldo_WalletTransaction.Add(_WalletTransaction);
                    _context.SaveChanges();
                }

               

                //_context.SaveChanges();
                response.IsSuccess = true;
                response.Result = wallet.Balance;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"{ex.Message}\n {ex.InnerException?.Message}";
            }
            return response;
        }

        public async Task<ResponseDto> GetAllWalletsAsync(string customerId)
        {
            var response = new ResponseDto();
            try
            {
                var wallets = await _context.Fieldo_Wallet
                    .Where(w => w.CustomerId == customerId && w.IsActive && !w.IsDeleted)
                    .ToListAsync();

                if (wallets == null || wallets.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "No wallets found for this user";
                    return response;
                }

                response.IsSuccess = true;
                response.Result = wallets;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"{ex.Message}\n {ex.InnerException?.Message}";
            }
            return response;
        }

    }

}
