using FluentValidation;
using mail_sms_notification_service.Entities;
using System.Linq;
using System.Text.RegularExpressions;

namespace mail_sms_notification_service.Validators;

public class SmsRequestValidator : AbstractValidator<SmsRequestVm>
{
    public SmsRequestValidator()
    {
        RuleFor(x => x.FromName)
            .NotEmpty().WithMessage("Sender Name is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Receiver phone number required.")
            .Must(phoneNumbers => phoneNumbers.All(phone => IsValidPhoneNumber(phone)))
            .WithMessage("Invalid phone number format. Phone numbers must be 11 characters or less including the country code.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message body is required.");
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return phoneNumber.Length <= 11 && Regex.IsMatch(phoneNumber, @"^\+?\d{1,11}$");
    }
}
