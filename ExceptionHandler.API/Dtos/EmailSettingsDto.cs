using ExceptionHandler.API.Intefaces;
using System;

namespace ExceptionHandler.API.Dtos
{
    public class EmailSettingsDto : IEmailSettings
    {
        public Guid EimailSettingsId { get; set; }
        public Guid RegistredApplicationId { get; set; }
        public Guid EmailTypeId { get; set; }
        public string FromEmailAddress { get; set; }
        public string FromName { get; set; }
        public string ToEmailAddress { get; set; }
        public string ToNames { get; set; }
        public string CcEmailAddresses { get; set; }
        public string CcNames { get; set; }
        public string BCcEmailAddresses { get; set; }
        public string BCcNames { get; set; }
        public string EmailSubject { get; set; }
        public bool IsHtml { get; set; }
        public bool AddAttachment { get; set; }
        public string SMTPServer { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public int SMTPPort { get; set; }
        public string TitleTemplate { get; set; }
        public string Template { get; set; }
        public string TemplateStyle { get; set; }
        public string TemplateHead { get; set; }
        public string TemplateBody { get; set; }
        public string LookupColumn { get; set; }
        public bool EnableSsl { get; set; }
    }
}
