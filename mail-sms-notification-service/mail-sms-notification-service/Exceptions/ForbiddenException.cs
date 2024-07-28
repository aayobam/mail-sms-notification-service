namespace mail_sms_notification_service.Exceptions;

public class ForbiddenException : DomainException
{

    public ForbiddenException() :
      base($"UnauthorizedAccess: user is not allowed to access this resource.", "Access Denied")
    {

    }

    public ForbiddenException(string user)
        : base($"UnauthorizedAccess: {user} is not allowed to access this resource.", "Access Denied")
    { }
}
