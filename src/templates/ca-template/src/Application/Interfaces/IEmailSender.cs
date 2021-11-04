// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string sendTo, string from, string subject, string body);
}
