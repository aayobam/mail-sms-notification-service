using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
namespace mail_sms_notification_service.Filters;

public class ValidationFilter<T> : ActionFilterAttribute, IActionFilter where T : class
{
    public async override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var validator = filterContext.HttpContext.RequestServices.GetService<IValidator<T>>();
        var objectToValidate = filterContext.ActionArguments["model"] as T;
        if (objectToValidate == null)
        {
            filterContext.Result = new BadRequestObjectResult("Invalid model");
            return;
        }
        var validationResult = await validator.ValidateAsync(objectToValidate);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(error => new ErrorResponseVm
            {
                Code = error.ErrorCode,
                Description = error.ErrorMessage
            }).ToList();

            filterContext.Result = new BadRequestObjectResult(errors);
            return;
        }
        else
        {
            base.OnActionExecuting(filterContext);
        }
    }
}