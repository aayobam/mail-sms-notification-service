using mail_sms_notification_service.configuration;
using Microsoft.Extensions.Options;
using MailKit.Security;
using System.Net;
using SecurePayEmailService.Helper;
using mail_sms_notification_service.Entities;
using MimeKit;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace mail_sms_notification_service.Services;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly SmtpConfiguration _smtpConfig;

    public NotificationService(ILogger<NotificationService> logger, IOptions<SmtpConfiguration> smtpConfig)
    {
        _logger = logger;
        _smtpConfig = smtpConfig.Value;
    }

    public async Task<GenericResponse<MailRequestVm>> SendEmailAsync(MailRequestVm request)
    {
        _logger.LogInformation($"\n----- preparing to send mail to {request.ReceiverEmail} | {DateTime.UtcNow} -----\n".ToUpper());

        var builder = new BodyBuilder();
        string fromEmail = $"{request.FromName} {_smtpConfig.FromEmail}";

        var mailContext = new MimeMessage()
        {
            From = { MailboxAddress.Parse(fromEmail) },
            To = { MailboxAddress.Parse(request.ReceiverEmail) },
            Subject = request.EmailSubject,
        };

        if (request.Attachments != null)
        {
            foreach (var attachment in request.Attachments)
            {
                byte[]? fileByte;
                using (var memoryStream = new MemoryStream())
                {
                    await attachment.CopyToAsync(memoryStream);
                    fileByte = memoryStream.ToArray();
                }
                builder.Attachments.Add(attachment.FileName, fileByte, MimeKit.ContentType.Parse(attachment.ContentType));
            }
        }

        builder.HtmlBody = request.HtmlEmailMessage;
        mailContext.Body = builder.ToMessageBody();
        var smtp = new MailKit.Net.Smtp.SmtpClient();
        await smtp.ConnectAsync(_smtpConfig.SmtpHost, _smtpConfig.SmtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_smtpConfig.SmtpUser, _smtpConfig.SmtpPass);
        try
        {
            await smtp.SendAsync(mailContext);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation($"\n----- Mail successfully sent to {request.ReceiverEmail} | {DateTime.UtcNow} -----\n".ToUpper());

            return new GenericResponse<MailRequestVm>
            {
                Data = null,
                Message = $"Mail successfully sent to {request.ReceiverEmail}",
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }
        catch (Exception exception)
        {
            _logger.LogInformation($"\n----- {exception.Message}: Could not send mail to {request.ReceiverEmail} | {DateTime.UtcNow} -----\n".ToUpper());
            return new GenericResponse<MailRequestVm>()
            {
                Data = null,
                Message = $"{exception.Message}: Could not send mail to {request.ReceiverEmail}",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    public async Task<GenericResponse<SmsBaseResponse>> SendSmsAsync(SmsRequestVm model)
    {
        var url = $"{_smtpConfig.BaseUrl}sms/2/text/advanced";

        _logger.LogInformation($"\n----- PREPARING TO SEND SMS TO {string.Join(", ", model.PhoneNumber)} : {url} | {DateTime.UtcNow} -----\n");

        var client = new RestClient();
        var request = new RestRequest(url, Method.Post);
        request.AddHeader("Authorization", $"App {_smtpConfig.ApiKey}");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");

        _logger.LogInformation($"\n----- PREPARING TO SEND SMS TO {string.Join(", ", model.PhoneNumber)}: | {DateTime.UtcNow} -----\n");

        var smsInfobitRequest = new InfobitsSmsRequestVm();
        var smsMessages = new List<Message>();

        smsMessages.Add(new Message
        {
            Destinations = model.PhoneNumber
            .Select(phoneNumber => new Destination
            {
                To = phoneNumber
            }).ToList(),
            From = model.FromName,
            Text = model.Message
        });

        smsInfobitRequest.Messages = smsMessages;

        request.AddParameter("application/json", smsInfobitRequest, ParameterType.RequestBody);

        _logger.LogInformation($"\n------- INFOBIPS SMS REQUEST: {JsonConvert.SerializeObject(smsInfobitRequest)} -------\n");


        var smsBaseResponse = new SmsBaseResponse();
        var messages = new List<Messages>();

        try
        {
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<SmsBaseResponse>(response.Content!);

                _logger.LogInformation($"\n---------- sms successfully sent to {string.Join(", ", model.PhoneNumber)} | {DateTime.UtcNow} ---------\n".ToUpper());

                _logger.LogInformation($"\n---------- sms response: {JsonConvert.SerializeObject(data)} | {DateTime.UtcNow} ---------\n".ToUpper());

                smsBaseResponse.Messages = messages;
                var serializedResponse = JsonConvert.SerializeObject(smsBaseResponse);

                _logger.LogInformation($"\n---------- {serializedResponse} | {string.Join(", ", model.PhoneNumber)} | {DateTime.UtcNow} ---------\n".ToUpper());

                return new GenericResponse<SmsBaseResponse>
                {
                    Success = true,
                    Message = $"sms successfully sent to {string.Join(", ", model.PhoneNumber)}",
                    StatusCode = HttpStatusCode.OK
                };
            }

            else
            {
                var data = JsonConvert.DeserializeObject<Messages>(response.Content!);

                messages.Add(new Messages
                {
                    MessageId = data!.MessageId,
                    Status = new Status()
                    {
                        Description = data.Status.Description,
                        GroupId = data.Status.GroupId,
                        GroupName = data.Status.GroupName,
                        Id = data.Status.Id,
                        Name = data.Status.Name
                    },
                    To = data.To
                });

                smsBaseResponse.Messages = messages;
                var serializedResponse = JsonConvert.SerializeObject(smsBaseResponse);

                _logger.LogInformation($"\n---------- {serializedResponse} | {string.Join(", ", model.PhoneNumber)} | {DateTime.UtcNow} ---------\n".ToUpper());

                return new GenericResponse<SmsBaseResponse>
                {
                    Success = false,
                    Message = serializedResponse,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
        catch (Exception exception)
        {

            _logger.LogInformation($"\n---------- {exception.Message}: service unavailable | {DateTime.UtcNow} ---------\n");

            return new GenericResponse<SmsBaseResponse>()
            {
                Success = false,
                Message = "service unavailable.",
                StatusCode = HttpStatusCode.InternalServerError,
            };
        }

    }
}
