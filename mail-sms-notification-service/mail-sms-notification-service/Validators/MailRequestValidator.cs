using FluentValidation;
using mail_sms_notification_service.Entities;

namespace mail_sms_notification_service.Validators;

public class MailRequestValidator : AbstractValidator<MailRequestVm>
{
    public MailRequestValidator()
    {
        RuleFor(x => x.FromName)
            .NotEmpty().WithMessage("Sender Name is required.");

        RuleFor(x => x.ReceiverEmail)
            .NotEmpty().WithMessage("Receiver Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.EmailSubject)
            .NotEmpty().WithMessage("Email Subject is required.");

        RuleFor(x => x.HtmlEmailMessage)
            .NotEmpty().WithMessage("Email message is required.")
            .Matches("<[^>]+>").WithMessage("Invalid HTML content.");
    }
}
