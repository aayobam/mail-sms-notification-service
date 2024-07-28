using System;

namespace mail_sms_notification_service.Exceptions;

public class DomainException : Exception
{
    public string Code { get; set; }
    public DomainException()
    { }

    public DomainException(string message)
        : base(message)
    { }

    public DomainException(string message, string code)
        : base(message)
    {
        Code = code;
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
