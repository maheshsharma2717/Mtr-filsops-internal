using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IWalletService
    {
        Task<ResponseDto> ChargeAmountAsync(string customerId, long amount, string currency, int? taskId);
        Task<ResponseDto> GetWalletBalanceAsync(string customerId, string currency);
        Task<ResponseDto> AddAmountAsync(string customerId, double amount, string currency, string requestId);
        Task<ResponseDto> GetAllWalletsAsync(string customerId);
    }
}
