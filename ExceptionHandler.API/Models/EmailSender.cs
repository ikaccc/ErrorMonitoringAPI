using ExceptionHandler.API.Common;
using ExceptionHandler.API.Enums;
using ExceptionHandler.API.Intefaces;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExceptionHandler.API.Models
{
    public class EmailSender : IEmailSender
    {
        private readonly IDataManager _dataManager;

        public EmailSender(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public enum AddressTypes
        {
            To,
            Cc,
            Bcc
        }

        public Guid ExceptionId { get; set; }

        //get email settings by logged/requested access token
        public async Task SendMailProccess(string accessToken, Payload data, EmailType mailType, RegistredApplications registredApplication)
        {
            IEmailSettings emailSettings = null;
            switch (mailType)
            {
                case EmailType.EveryOccurrence:
                    emailSettings = await _dataManager.GetEmailSettings(accessToken, Constants.EveryOccurrence);
                    break;
                case EmailType.SendDailySummary:
                    emailSettings = await _dataManager.GetEmailSettings(accessToken, Constants.SendDailySummary);
                    break;
                case EmailType.HighOccurrenceRate:
                    emailSettings = await _dataManager.GetEmailSettings(accessToken, Constants.HighOccurrenceRate);
                    break;
                case EmailType.ItemResolved:
                    emailSettings = await _dataManager.GetEmailSettings(accessToken, Constants.ItemResolved);
                    break;
                case EmailType.ItemReactivated:
                    emailSettings = await _dataManager.GetEmailSettings(accessToken, Constants.ItemReactivated);
                    break;
                case EmailType.NewVersion:
                    emailSettings = await _dataManager.GetEmailSettings(accessToken, Constants.NewVersion);
                    break;
                case EmailType.ItemReopened:
                    emailSettings = await _dataManager.GetEmailSettings(accessToken, Constants.ItemReopened);
                    break;
                default:
                    emailSettings = await _dataManager.GetEmailSettings(accessToken, Constants.NewItem);
                    break;
            }

            if (emailSettings != null)
            {
                var message = "";

                var errorLevelMsg = "";

                switch (data.Data.Level)
                {
                    case ErrorLevel.Debug:
                        errorLevelMsg = "<strong><span style=\"color: red;\">" + data.Data.Level.Value + "</span></strong>";
                        break;
                    case ErrorLevel.Info:
                        errorLevelMsg = "<strong><span style=\"color: green;\">" + data.Data.Level.Value + "</span></strong>";
                        break;
                    case ErrorLevel.Warning:
                        errorLevelMsg = "<strong><span style=\"color: blue;\">" + data.Data.Level.Value + "</span></strong>";
                        break;
                    case ErrorLevel.Error:
                        errorLevelMsg = "<strong><span style=\"color: red;\">" + data.Data.Level.Value + "</span></strong>";
                        break;
                    case ErrorLevel.Critical:
                        errorLevelMsg = "<strong><span style=\"color: red;\">" + data.Data.Level.Value + "</span></strong>";
                        break;
                    case ErrorLevel.JavaScript:
                        errorLevelMsg = "<strong><span style=\"color: yellow;\">" + data.Data.Level.Value + "</span></strong>";
                        break;
                    default:
                        errorLevelMsg = "";
                        break;
                }


                message = emailSettings.Template;
                message = message.Replace("[[HEAD]]", emailSettings.TemplateHead);
                message = message.Replace("[[BODY]]", emailSettings.TemplateBody);
                message = message.Replace("[[STYLE]]", "");
                message = message.Replace("[[ProjectName]]", registredApplication.ApplicationName)
                    .Replace("[[Framework]]", data.Data.Framework)
                    .Replace("[[Language]]", data.Data.Language)
                    .Replace("[[ErrorLevel]]", errorLevelMsg)
                    .Replace("[[UserAgent]]", data.Data.Person.UserAgent)
                    .Replace("[[UserLogin]]", data.Data.Person.UserName)
                    .Replace("[[UserId]]", data.Data.Person.Id)
                    .Replace("[[IpAddress]]", data.Data.Person.IpAddress)
                    .Replace("[[Environment]]", data.Data.Environment)
                    .Replace("[[NotifierName]]", data.Data.Notifier.ContainsKey("name") ? data.Data.Notifier["name"] : string.Empty)
                    .Replace("[[NotifierVersion]]", data.Data.Notifier.ContainsKey("version") ? data.Data.Notifier["version"] : string.Empty)
                    .Replace("[[Platform]]", data.Data.Platform)
                    .Replace("[[Uuid]]", data.Data.Uuid)
                    .Replace("[[CodeVersion]]", Assumption.AssertNotNullOrWhiteSpaceBool(data.Data.CodeVersion) ? data.Data.CodeVersion : "unspecified")
                    .Replace("[[Host]]", data.Data.Server?.Host ?? "unspecified")
                    .Replace("[[Timestamp]]", data.Data.Timestamp.ToString())
                    .Replace("[[TimestampDate]]", data.Data.sysDate.ToString("dd/MMM/yyyy"))
                    .Replace("[[TimestampTime]]", (data.Data.sysDate.ToString("HH:mm:ss tt") + " UTC(+ 00:00)"))
                    .Replace("[[ExceptionMessage]]", "<strong><span style=\"color: red;\">" + (data.Data.Level == ErrorLevel.JavaScript ? data?.Data?.Body?.Message?.Body.Split('\n')[0] : data.Data.Body?.Trace?.Exception?.Message) + "</span></strong>")
                    .Replace("[[CompanyInfo]]", "Infinite Solutions, Luj Paster 1 - Skopje, Macedonia")
                    .Replace("[[FRAMELINES]]", data.Data.Level == ErrorLevel.JavaScript ? string.Join("\n", data?.Data?.Body?.Message?.Body.Split('\n').Skip(1)) : data.Data.Body?.Trace?.Exception?.StackTrace);
                ExceptionId = data.Data.Id;
                SendMail(emailSettings, message, data.Data.Id, data.Data.Environment);
            }
        }

        //sedn mail using smtp
        public void SendMail(IEmailSettings emailSettings, string message, Guid dataId, string subject)
        {
            try
            {
                if (Assumption.AssertNotNullOrWhiteSpaceBool(emailSettings.FromEmailAddress) &&
                    Assumption.AssertNotNullOrWhiteSpaceBool(emailSettings.ToEmailAddress))
                {
                    var mail = new MailMessage
                    {
                        Subject = $"[{subject}]" + emailSettings.EmailSubject,
                        Body = message,
                        IsBodyHtml = emailSettings.IsHtml,
                        From = emailSettings.FromName.Trim().Length == 0
                            ? new MailAddress(emailSettings.FromEmailAddress)
                            : new MailAddress(emailSettings.FromEmailAddress, emailSettings.FromName)
                    };


                    AddEmailAddresses(mail, AddressTypes.To, emailSettings.ToNames, emailSettings.ToEmailAddress);
                    AddEmailAddresses(mail, AddressTypes.Cc, emailSettings.CcNames, emailSettings.CcEmailAddresses);
                    AddEmailAddresses(mail, AddressTypes.Bcc, emailSettings.BCcNames, emailSettings.BCcEmailAddresses);

                    var emailAuthenticationInfo = new NetworkCredential(emailSettings.SMTPUserName, emailSettings.SMTPPassword);
                    var client = new SmtpClient(emailSettings.SMTPServer)
                    {
                        UseDefaultCredentials = false,
                        Credentials = emailAuthenticationInfo,
                        EnableSsl = emailSettings.EnableSsl,
                        Port = emailSettings.SMTPPort
                    };

                    client.SendCompleted += new
                        SendCompletedEventHandler(SendCompletedCallback);

                    client.SendAsync(mail, "Message");
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private static void AddEmailAddresses(MailMessage message, AddressTypes addressType, string names,
            string emailAddresses)
        {
            MailAddressCollection mailsToAddTo = null;
            if (addressType == AddressTypes.To)
            {
                mailsToAddTo = message.To;
            }
            else if (addressType == AddressTypes.Cc)
            {
                mailsToAddTo = message.CC;
            }
            else if (addressType == AddressTypes.Bcc)
            {
                mailsToAddTo = message.Bcc;
            }

            if (Assumption.AssertNotNullOrWhiteSpaceBool(emailAddresses))
            {
                emailAddresses = emailAddresses.Replace("[", "");
                emailAddresses = emailAddresses.Replace("\"", "");
                emailAddresses = emailAddresses.Replace("]", "");

                string[] splitter = { ",", ";" };
                string[] displayNames = names.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                string[] emails = emailAddresses.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < emails.Length; i++)
                {
                    string currentEmailAddress = emails[i].Trim();
                    string currentEmailDisplayName = "";
                    if (displayNames.GetUpperBound(0) >= i)
                    {
                        currentEmailDisplayName = displayNames[i].TrimStart();
                    }

                    mailsToAddTo.Add(currentEmailDisplayName == ""
                          ? new MailAddress(currentEmailAddress)
                          : new MailAddress(currentEmailAddress, currentEmailDisplayName));
                }
            }
        }

        private async void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            string token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                await _dataManager.SetMailSendFlag(ExceptionId, e.Error.Message);
            }
            else
            {
                try
                {
                    await _dataManager.SetMailSendFlag(ExceptionId);
                }
                catch (System.Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }
    }
}
