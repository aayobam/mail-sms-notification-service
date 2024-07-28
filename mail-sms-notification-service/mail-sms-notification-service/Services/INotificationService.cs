using mail_sms_notification_service.Entities;
using SecurePayEmailService.Helper;
using System.Threading.Tasks;

namespace mail_sms_notification_service.Services;

public interface INotificationService
{
    Task<GenericResponse<MailRequestVm>> SendEmailAsync(MailRequestVm request);
    Task<GenericResponse<SmsBaseResponse>> SendSmsAsync(SmsRequestVm request);
}
