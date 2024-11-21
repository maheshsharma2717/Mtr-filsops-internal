using Application.Models;
using MTR_Fieldo_API.Models.Dto;
using Square.Models;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IPaymentService
    {
        Task<ResponseDto> CreatePaymentIntent(Fieldo_UserDetails user, string stripeCustomerId, PaymentDto payment);
        Task<ResponseDto> UpdatePaymentStatus(PaymentstatusDto payment);
        Task<ResponseDto> CreateStripeCustomer(string Name, string Email);
        Task<ResponseDto> AttachPaymentMethod(Fieldo_UserDetails fieldo_User, string paymentId);
        Task<ResponseDto> CreateStripePaymentMethod(Fieldo_UserDetails fieldo_User, CardDto card);
        Task<ResponseDto> DetachPaymentMethod(string paymentId);
        Task<ResponseDto> CreateSquareCustomer(SquareCustomerDto createCustomer);
        Task<ResponseDto> CreateSquarePayment(PaymentRequestDto paymentRequestDto, Fieldo_UserDetails UserDetail);
        Task<ResponseDto> CancelSquarePayment(string PaymentId);
        Task<ResponseDto> AddSquareCard(Fieldo_UserDetails fieldo_User, SquareCardDto squareCardDto);
        Task<ResponseDto> DisableSquareCard(Fieldo_UserDetails fieldo_User, string cardId);
        Task<ResponseDto> CompleteSquarePayment(Fieldo_UserDetails fieldo_User,string paymentId, string VersionToken);
        Task<ResponseDto> UpdateSquarePayment(UpdatePaymentDto updatePaymentDto);
        Task<ResponseDto> GetMyTransactionHistory(Fieldo_UserDetails fieldo_User, int domainId);
        Task<ResponseDto> GetMyMonthlySpent(Fieldo_UserDetails fieldo_User);
        Task<ResponseDto> GetMyTotalSpent(Fieldo_UserDetails fieldo_User);
        Task<ResponseDto> GetMyEarning(Fieldo_UserDetails fieldo_User);
        Task<ResponseDto> GetMyTotalSpentByMonthAsync(Fieldo_UserDetails fieldo_User, int year, int month);
        Task<object> GetMyEarningByUserId(int userId);

    }
}
