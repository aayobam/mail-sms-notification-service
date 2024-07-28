using System.Collections.Generic;
using System.Net;

namespace SecurePayEmailService.Helper;

public class GenericResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

public class Messages
{
    public string MessageId { get; set; } = null!;

    public Status Status { get; set; } = null!;

    public string To { get; set; } = null!;
}

public class SmsBaseResponse
{
    public List<Messages> Messages { get; set; } = null!;
}

public class Status
{
    public string Description { get; set; } = null!;

    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
