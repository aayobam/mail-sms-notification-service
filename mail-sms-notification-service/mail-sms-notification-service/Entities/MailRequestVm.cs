using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mail_sms_notification_service.Entities;

public class MailRequestVm
{
    public string FromName { get; set; } = null!;
    public string ReceiverEmail { get; set; } = null!;
    public string EmailSubject { get; set; } = null!;
    public string HtmlEmailMessage { get; set; } = null!;
    public List<IFormFile>? Attachments { get; set; }
}
