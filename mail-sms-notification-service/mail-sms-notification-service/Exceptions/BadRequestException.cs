namespace mail_sms_notification_service.Exceptions;

public class BadRequestException : DomainException
{
    public BadRequestException(string message) : base(message)
    {

    }
    public BadRequestException(string message, string code) : base(message, code)
    {

    }
}
