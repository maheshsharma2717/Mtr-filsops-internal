using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IBankService
    {
        Task<ResponseDto> GetAllBanks();
        Task<ResponseDto> GetAllWorkerBanks();
        Task<ResponseDto> AddUserBankDetails(BankDetailsRequestDto bankDetailsRequestDto);
        Task<ResponseDto> ChangeActiveBankStatus(int workerBankDetailId, int userId);
        Task<ResponseDto> GetBankDetailsByUserId(int userId);
        Task<ResponseDto> GetBankByIdAndUserID(int bankId, int UserId,int domainId);
        Task<ResponseDto> AddBank(BankRequestDto bankRequest);
        Task<ResponseDto> UpdateBank(BankRequestDto bankRequest);
        Task<ResponseDto> GetBankById(int bankId);
        Task<ResponseDto> DeleteBank(int bankId);
    }
}
