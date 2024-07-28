using mail_sms_notification_service.Exceptions;
using System;
using System.Linq;

namespace mail_sms_notification_service.Extensions;

public static class ErrorResponseExtensions
{
    public static ErrorResponseVm ToErrorResponse(this NotFoundException e)
    {
        return new ErrorResponseVm()
        {
            Code = e.Code,
            Description = e.Message
        };
    }

    public static ErrorResponseVm ToErrorResponse(this BadRequestException e)
    {
        return new ErrorResponseVm()
        {
            Code = e.Code,
            Description = e.Message
        };
    }

    public static ErrorResponseVm ToErrorResponse(this ServiceUnavailableException e)
    {
        return new ErrorResponseVm()
        {
            Code = e.Code,
            Description = e.Message
        };
    }

    public static ErrorResponseVm ToErrorResponse(this DomainException e)
    {
        return new ErrorResponseVm()
        {
            Code = e.Code,
            Description = e.Message
        };
    }


    public static ErrorResponseVm ToErrorResponse(this Exception e)
    {
        return new ErrorResponseVm()
        {
            Code = "SYSTEM_ERROR",
            Description = $"Unexpected error occured please try again or confirm current operation status"
        };
    }


    public static ErrorResponseVm ToErrorResponse(this ValidationException e)
    {
        var msg = string.Empty;
        foreach (var item in e.Failures)
        {
            var errors = item.Value.ToList();
            msg += errors.Aggregate("",
                (current, next) => $"{current} {next}");


        }

        return new ErrorResponseVm()
        {
            Code = e.Code,
            Description = $"{e.Message} {msg}"
        };
    }


}
