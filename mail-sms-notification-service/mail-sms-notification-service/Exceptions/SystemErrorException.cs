namespace mail_sms_notification_service.Exceptions;


public class SystemErrorException : DomainException
{
    public SystemErrorException()
        : base("An Unexpected error occured please try again or confirm current operation status", "SYSTEM_ERROR")
    {

    }

    public SystemErrorException(string message)
        : base(message, "SYSTEM_ERROR")
    { }
}
