using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MTR_Fieldo_API.Models.Dto;

public class ValidateDomainIdFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionArguments.TryGetValue("domainId", out var value) || value == null || !(value is int domainId) || domainId == 0)
        {
            context.Result = new JsonResult(new ResponseDto
            {
                IsSuccess = false,
                Message = "domainId is required and cannot be null, empty, or zero."
            })
            {
                StatusCode = 400
            };
            return;
        }

        base.OnActionExecuting(context);
    }
}
