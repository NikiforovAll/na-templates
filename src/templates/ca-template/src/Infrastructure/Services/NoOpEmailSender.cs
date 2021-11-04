// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Infrastructure.Services;

using Microsoft.Extensions.Logging;
using NikiforovAll.CA.Template.Application.Interfaces;

public partial class NoOpEmailSender : IEmailSender
{
    private readonly ILogger<NoOpEmailSender> logger;

    public NoOpEmailSender(ILogger<NoOpEmailSender> logger) => this.logger = logger;

    [LoggerMessage(
        0,
        LogLevel.Warning,
        "Sending email to {SendTo} from {From} with subject {Subject}. [{BodyLength}]")]
    partial void LogEmailSending(string sendTo, string from, string subject, int bodyLength);

    public Task SendEmailAsync(string sendTo, string from, string subject, string body)
    {
        //var emailClient = new SmtpClient("localhost");
        //var message = new MailMessage
        //{
        //    From = new MailAddress(from),
        //    Subject = subject,
        //    Body = body
        //};
        //message.To.Add(new MailAddress(to));
        //await emailClient.SendMailAsync(message);

        this.LogEmailSending(sendTo, from, subject, body.Length);

        return Task.CompletedTask;
    }
}
