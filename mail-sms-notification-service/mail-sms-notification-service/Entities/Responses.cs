using Newtonsoft.Json;

namespace SecurePayEmailService.Entities.Response;

public partial class Trace
{
    [JsonProperty("appPlatform")]
    public string? AppPlatform { get; set; }

    [JsonProperty("traceId")]
    public string? TraceId { get; set; }

    [JsonProperty("appVersion")]
    public object? AppVersion { get; set; }

    [JsonProperty("ipAddress")]
    public string? IpAddress { get; set; }
}