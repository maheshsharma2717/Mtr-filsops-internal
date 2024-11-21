using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTR_Fieldo_API.Service.IService;
using Square.Models;
using Square.Utilities;
using Stripe;
using Stripe.Forwarding;
using Stripe.Terminal;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MTR_Fieldo_API.Controllers
{
    [Route("webhook")]
    [ApiController]
    public class WebhookController : Controller
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly IWebhookService _webhookService;
        private const string NOTIFICATION_URL = "https://example.com/webhook";
        private const string SIGNATURE_KEY = "asdf1234";


        public WebhookController(ILogger<WebhookController> logger, IWebhookService webhookService)
        {
            _logger = logger;
            _webhookService = webhookService;
        }

        //const string stripeSecret = "sk_test_51P8GHRSBJd9qPRs4hLSo6YZ4uF6F2RmTlYlmMa93CmZdK8LL1t1qm2oXTy7kGtzWTspxdX4NhTOCCSuShfFvjrWM00ssScEtyb";

        //[HttpPost("ReceiveStripeWebhook")]
        //public ActionResult ReceiveStripeWebhook()
        //{
        //    try
        //    {
        //        var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();

        //        var stripeEvent = EventUtility.ConstructEvent(json,
        //            Request.Headers["Stripe-Signature"], stripeSecret);

        //        PaymentIntent intent = null;

        //        switch (stripeEvent.Type)
        //        {
        //            case "payment_intent.succeeded":
        //                intent = (PaymentIntent)stripeEvent.Data.Object;
        //                _webhookService.UpdateWebhookResponse(intent.Id, intent.Status);
        //                _logger.LogInformation("Succeeded: {ID}",intent.Id);

        //                // Fulfil the customer's purchase

        //                break;
        //            case "payment_intent.payment_failed":
        //                intent = (PaymentIntent)stripeEvent.Data.Object;
        //                _webhookService.UpdateWebhookResponse(intent.Id, intent.Status);
        //                _logger.LogInformation("Failure: {ID}", intent.Id);

        //                // Notify the customer that payment failed

        //                break;
        //            default:
        //                // Handle other event types

        //                break;
        //        }
        //        return new EmptyResult();

        //    }
        //    catch (StripeException e)
        //    {
        //        // Invalid Signature
        //        return BadRequest();
        //    }
        //}
        //[HttpPost("ReceiveSquareWebhook")]
        //public async Task<IActionResult> ReceiveSquareWebhook()
        //{
        //    try
        //    {
        //        var payment = await DeserializeRequest<Square.Models.Payment>(Request);
        //        var isValidSignature = await IsFromSquare(Request);

        //        if (isValidSignature)
        //        {
        //            _webhookService.UpdateSquareWebhookResponse(payment);
        //            return Ok();
        //        }
        //        else
        //        {
        //            return StatusCode(403);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log and handle errors appropriately
        //        Console.WriteLine($"Error handling webhook: {ex.Message}");
        //        return StatusCode(500, "An error occurred while processing the webhook.");
        //    }
        //}

        //private async Task<T> DeserializeRequest<T>(HttpRequest request)
        //{
        //    using (var reader = new StreamReader(request.Body, Encoding.UTF8))
        //    {
        //        var requestBody = await reader.ReadToEndAsync();
        //        // Use Newtonsoft.Json or System.Text.Json to deserialize JSON into object
        //        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(requestBody);
        //    }
        //}
        //private async Task<bool> IsFromSquare(HttpRequest request)
        //{
        //    using (var reader = new StreamReader(request.Body, Encoding.UTF8))
        //    {
        //        var signature = request.Headers["x-square-hmacsha256-signature"].ToString();
        //        var requestBody = await reader.ReadToEndAsync();
        //        return WebhooksHelper.IsValidWebhookEventSignature(requestBody, signature, SIGNATURE_KEY, NOTIFICATION_URL);
        //    }
        //}
    }
}