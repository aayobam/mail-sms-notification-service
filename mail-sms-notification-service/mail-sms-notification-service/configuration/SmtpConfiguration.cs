using Newtonsoft.Json;

namespace mail_sms_notification_service.configuration;

public class SmtpConfiguration
{
    [JsonProperty("FromEmail")]
    public string FromEmail { get; set; } = null!;

    [JsonProperty("SmtpHost")]
    public string SmtpHost { get; set; } = null!;

    [JsonProperty("SmtpPort")]
    public int SmtpPort { get; set; }

    [JsonProperty("SmtpUser")]
    public string SmtpUser { get; set; } = null!;

    [JsonProperty("SmtpPassword")]
    public string SmtpPass { get; set; } = null!;

    public string BaseUrl { get; set; } = null!;

    public string ApiKey { get; set; } = null!;
}