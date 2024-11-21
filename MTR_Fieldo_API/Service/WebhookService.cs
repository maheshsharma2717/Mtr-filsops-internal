using Application.Models;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using Stripe;

namespace MTR_Fieldo_API.Service
{
    public class WebhookService : IWebhookService
    {

        private readonly MtrContext _context;
        private readonly ResponseDto _responseDto;
        private readonly IWalletService _walletService;
        public WebhookService(MtrContext context, IWalletService walletService)
        {
            _context = context;
            _responseDto = new ResponseDto();
            _walletService = walletService;
        }
    //    public async Task<ResponseDto> UpdateWebhookResponse(string intentId, string status)
    //    {
    //       var paymentIntent= _context.Fieldo_Payments.Where(x=>x.IntentId==intentId).FirstOrDefault();
    //        if (paymentIntent!=null) 
    //        {
    //            paymentIntent.Status= status;
    //        }
    //        _context.Fieldo_Payments.Update(paymentIntent);
    //        if(await _context.SaveChangesAsync()>0)
    //        {
    //            _walletService.AddAmountAsync(paymentIntent.UserId.ToString(), Convert.ToInt64( paymentIntent.Amount), paymentIntent.Currency);
    //        }
    //        return _responseDto;
    //    }

    //    public async Task<ResponseDto> UpdateSquareWebhookResponse(Square.Models.Payment payment)
    //    {
    //        var result= _context.Fieldo_Payments.Where(x => x.SquareId == payment.Id).FirstOrDefault();
    //        if (result != null)
    //        {
    //            result.Status = payment.Status;
    //        }
    //        _context.Fieldo_Payments.Update(result);

    //        if (await _context.SaveChangesAsync() > 0)
    //        {
    //            _walletService.AddAmountAsync(result.UserId.ToString(), Convert.ToInt64(result.Amount), result.Currency);
    //        }
    //        return _responseDto;
    //         }
    }
}
