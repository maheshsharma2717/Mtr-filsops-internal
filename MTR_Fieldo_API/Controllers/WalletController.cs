using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service;
using MTR_Fieldo_API.Service.IService;
using System.Security.Claims;

namespace MTR_Fieldo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IAuthenticateService _authenticateService;

        public WalletController(IWalletService walletService, IAuthenticateService authenticateService)
        {
            _walletService = walletService;
            _authenticateService = authenticateService;
        }

        [HttpPost("chargeamount")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> ChargeAmount([FromBody] ChargeAmountRequestDto request, int domainId)
        {
            Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            if (claim != null)
            {
                Fieldo_UserDetails userDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _walletService.ChargeAmountAsync(userDetail.Id.ToString(), request.Amount, request.Currency, request.TaskId);
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Claims not found"
                };
            }
        }

        [HttpPost("addamount")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AddAmount([FromBody] AddAmountRequestDto request, int domainId)
        {
            Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            if (claim != null)
            {
                Fieldo_UserDetails userDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _walletService.AddAmountAsync(userDetail.Id.ToString(), request.Amount, request.Currency, Guid.NewGuid().ToString());
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Claims not found"
                };
            }
        }

        [HttpGet("getwalletbalance")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetWalletBalance([FromQuery] string currency, int domainId)
        {
            Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            if (claim != null)
            {
                Fieldo_UserDetails userDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _walletService.GetWalletBalanceAsync(userDetail.Id.ToString(), currency);
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Claims not found"
                };
            }
        }
        [HttpGet("getallwallets")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetAllWallets(int domainId)
        {
            Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            if (claim != null)
            {
                Fieldo_UserDetails userDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _walletService.GetAllWalletsAsync(userDetail.Id.ToString());
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Claims not found"
                };
            }
        }
    }
}
