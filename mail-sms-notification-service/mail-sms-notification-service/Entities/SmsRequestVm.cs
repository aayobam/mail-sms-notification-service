using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mail_sms_notification_service.Entities;

public class SmsRequestVm
{
    public string FromName { get; set; } = null!;
    public List<string> PhoneNumber { get; set; } = null!;
    public string Message { get; set; } = null!;
}
