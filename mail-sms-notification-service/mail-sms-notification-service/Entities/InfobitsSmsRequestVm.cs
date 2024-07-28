using System.Collections.Generic;

namespace mail_sms_notification_service.Entities;

public class Destination
{
    public string To { get; set; }
}

public class Message
{
    public List<Destination> Destinations { get; set; }


    public string From { get; set; }
    public string Text { get; set; }
}

public class InfobitsSmsRequestVm
{
    public List<Message> Messages { get; set; }
}
