namespace mail_sms_notification_service.Exceptions;


public class UnAuthorizedException : DomainException
{
    public UnAuthorizedException(string message, string code = "UnAuthorized") : base(message, code)
    {

    }
}
