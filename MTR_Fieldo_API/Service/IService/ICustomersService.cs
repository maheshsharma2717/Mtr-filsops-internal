using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface ICustomersService
    {
        Task<ResponseDto> AddOrUpdateServiceCustomers(CustomerDto customer ,int domainID);
        Task<ResponseDto> GetAllCustomers(int doomainId);
        Task<ResponseDto> GetCustomerById(int id ,int domainId);
        Task<ResponseDto> DeleteCustomerById(int customerId, int domainId);
    }
}
