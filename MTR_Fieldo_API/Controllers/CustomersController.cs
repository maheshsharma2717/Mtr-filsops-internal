using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service;
using MTR_Fieldo_API.Service.IService;
using System.Security.Claims;

//information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MTR_Fieldo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly ResponseDto _responseDto;
        private readonly ICustomersService _customerService;
        public CustomersController(IAuthenticateService authenticateService, ICustomersService customerService)
        {
            _responseDto = new ResponseDto();
            _authenticateService = authenticateService;
            _customerService = customerService;
        }
        [HttpGet("getAllCustomer")]
        [ValidateDomainIdFilter]
         [Authorize]
        public async Task<ResponseDto> GetAllCustomer(int domainId)
        {
            return await _customerService.GetAllCustomers(domainId);
         

        }

        [HttpPost("AddOrUpdateCustomers")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AddOrUpdateCustomers(CustomerDto customer ,int domainID)
        {
            return await _customerService.AddOrUpdateServiceCustomers(customer ,domainID);

        }
        [HttpGet("getCustomerById")]
        [ValidateDomainIdFilter]
         [Authorize] 
        public async Task<ResponseDto> GetCustomerById(int customerId, int domainId)
        {
            return await _customerService.GetCustomerById(customerId, domainId);
        }

        [HttpDelete("deleteCustomer")]
        [Authorize] 
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> DeleteCustomer(int customerId, int domainId)
        {
            return await _customerService.DeleteCustomerById(customerId, domainId);
        }


    }

}
