using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service;
using MTR_Fieldo_API.Service.IService;

namespace MTR_Fieldo_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }
        [HttpGet]
        [Route("GetAllBanks")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetAllBanks(int domainId)
        {
            return await _bankService.GetAllBanks();
        }
        [HttpGet]
        [Route("GetAllWorkerBanks")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetAllWorkerBanks(int domainId)
        {
            return await _bankService.GetAllWorkerBanks();
        }


        [HttpPost]
        [Route("AddUserBankDetails")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AddUserBankDetails(BankDetailsRequestDto bankDetailsRequestDto, int domainId)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);
            bankDetailsRequestDto.WorkerId = userId;
            return await _bankService.AddUserBankDetails(bankDetailsRequestDto);
        }
        [HttpPut]
        [Route("ChangeActiveBankStatus")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> ChangeActiveBankStatus(int workerBankDetailId, int domainId)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);
            return await _bankService.ChangeActiveBankStatus(workerBankDetailId, userId);
        }

        [HttpGet]
        [Route("GetBankDetailsByUserId/{userId}")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetBankDetailsByUserId(int userId, int domainId)
        {
            return await _bankService.GetBankDetailsByUserId(userId);
        }

        [HttpGet]
        [Route("GetBankByIdAndUserID")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetBankByIdAndUserID(int UserId, int bankId ,int domainId)
        {
            return await _bankService.GetBankByIdAndUserID(bankId, UserId,domainId);
        }

        [HttpPost]
        [Route("AddBank")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AddBank(BankRequestDto bankRequest, int domainId)
        {
            return await _bankService.AddBank(bankRequest);
        }

        [HttpGet]
        [Route("GetBankById")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetBankById(int bankId, int domainId)
        {
            return await _bankService.GetBankById(bankId);
        }
        [HttpPut]
        [Route("UpdateBank")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateBank(BankRequestDto bankRequest, int domainId)
        {
            return await _bankService.UpdateBank(bankRequest);
        }

        [HttpDelete]
        [Route("DeleteBank")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> DeleteBank(int bankId, int domainId)
        {
            return await _bankService.DeleteBank(bankId);
        }
    }
}
