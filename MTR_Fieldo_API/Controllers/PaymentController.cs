using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using Square.Models;
using System.Security.Claims;

namespace MTR_Fieldo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ResponseDto _responseDto;
        private readonly IPaymentService _paymentService;
        private readonly IAuthenticateService _authenticateService;
        public PaymentController(IPaymentService paymentService, IAuthenticateService authenticateService)
        {
            _responseDto = new ResponseDto();
            _paymentService = paymentService;
            _authenticateService = authenticateService;
        }
        [HttpPost("createpaymentintent")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> CreatePaymentIntent(PaymentDto payment, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.CreatePaymentIntent(UserDetail, UserDetail.StripeCustomerId, payment);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpPost("updatepaymentstatus")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdatePaymentStatus(PaymentstatusDto payment, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.UpdatePaymentStatus(payment);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpPost("createstripepaymentmethod")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> CreateStripePaymentMethod(CardDto card, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.CreateStripePaymentMethod(UserDetail, card);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpPost("attachpaymentmethod")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AttachPaymentMethod(string paymentId, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.AttachPaymentMethod(UserDetail, paymentId);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpPost("detachpaymentmethod")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> DetachPaymentMethod(string paymentId, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.DetachPaymentMethod(paymentId);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpPost("createsquarepayment")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> CreateSquarePayment([FromBody] PaymentRequestDto paymentRequestDto, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.CreateSquarePayment(paymentRequestDto, UserDetail);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpPost("updatesquarepayment")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateSquarePayment([FromBody] UpdatePaymentDto updatePaymentDto, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.UpdateSquarePayment(updatePaymentDto);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpPost("cancelsquarepayment")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> CancelSquarePayment(string paymentId, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.CancelSquarePayment(paymentId);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpPost("addsquarecard")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AddSquareCard(SquareCardDto squareCardDto, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.AddSquareCard(UserDetail, squareCardDto);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpPost("disablesquarecard")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> DisableSquareCard(string paymentId, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.DisableSquareCard(UserDetail, paymentId);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpPost("completesquarepayment")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> CompleteSquarePayment(string paymentId,string VersionToken, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.CompleteSquarePayment(UserDetail, paymentId, VersionToken);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpGet("getmytransactionhostory")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetMyTransactionHistory(int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.GetMyTransactionHistory(UserDetail, domainId);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

       [HttpGet("getmylastthirtydaysspent")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetMyMonthlySpent(int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.GetMyMonthlySpent(UserDetail);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpGet("getmytotalyspent")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetMyTotalSpent(int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.GetMyTotalSpent(UserDetail);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpGet("getmytotalspentbymonth")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetMyTotalSpentByMonthAsync(int year, int month, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.GetMyTotalSpentByMonthAsync(UserDetail, year, month);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpGet("getmyearning")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetMyEarning(int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _paymentService.GetMyEarning(UserDetail);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
   
    }

}