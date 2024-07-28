namespace mail_sms_notification_service.Exceptions;


public class ServiceUnavailableException : DomainException
{
    public ServiceUnavailableException() : base("An Unexpected error occured please try again or confirm current operation status", "Service Unavailable")
    {

    }

    public ServiceUnavailableException(string message, string code) : base(message, code)
    {

    }
}
