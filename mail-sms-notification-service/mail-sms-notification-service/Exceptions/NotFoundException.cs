namespace mail_sms_notification_service.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" identified with ({key}) was not found.", "NotFound")
    {

    }

    public NotFoundException(string message) : base(message, "NotFound")
    {

    }
}
