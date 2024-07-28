using mail_sms_notification_service.Entities;
using mail_sms_notification_service.Filters;
using mail_sms_notification_service.Services;
using mail_sms_notification_service.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecurePayEmailService.Helper;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace mail_sms_notification_service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("send-mail")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<>))]
    [ValidationFilter<MailRequestVm>]
    [SwaggerOperation(Summary = "Send mail to user supplied email.")]
    public async Task<ActionResult<GenericResponse<MailRequestVm>>> SendMail([FromForm] MailRequestVm model)
    {
        var emailResponse = await _notificationService.SendEmailAsync(model);
        return Ok(emailResponse);
    }

    [HttpPost("send-sms")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<>))]
    [ValidationFilter<SmsRequestVm>]
    [SwaggerOperation(Summary = "Send Sms to user phone number.")]
    public async Task<ActionResult<GenericResponse<SmsRequestVm>>> SendSms([FromBody] SmsRequestVm model)
    {
        var smsResponse = await _notificationService.SendSmsAsync(model);
        return Ok(smsResponse);
    }
}
