using ExceptionHandler.API.Enums;
using ExceptionHandler.API.Models;
using System;
using System.Threading.Tasks;

namespace ExceptionHandler.API.Intefaces
{
    public interface IEmailSender
    {
        //get email settings by logged/requested access token then send mail via SendMail function
        Task SendMailProccess(string accessToken, Payload data, EmailType emailType, RegistredApplications registredApplication);

        //all settings are already passed from SendMailProcess (from db). send mail using smtp
        void SendMail(IEmailSettings emailSettings, string message, Guid dataId, string subject);
    }
}
