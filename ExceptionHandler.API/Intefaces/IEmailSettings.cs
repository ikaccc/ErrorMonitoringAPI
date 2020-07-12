using System;

namespace ExceptionHandler.API.Intefaces
{
    //fake interface
    public interface IEmailSettings
    {
        Guid EimailSettingsId { get; set; }
        Guid RegistredApplicationId { get; set; }
        Guid EmailTypeId { get; set; }
        string FromEmailAddress { get; set; }
        string FromName { get; set; }
        string ToEmailAddress { get; set; }
        string ToNames { get; set; }
        string CcEmailAddresses { get; set; }
        string CcNames { get; set; }
        string BCcEmailAddresses { get; set; }
        string BCcNames { get; set; }
        string EmailSubject { get; set; }
        bool IsHtml { get; set; }
        bool AddAttachment { get; set; }
        string SMTPServer { get; set; }
        string SMTPUserName { get; set; }
        string SMTPPassword { get; set; }
        int SMTPPort { get; set; }
        string TitleTemplate { get; set; }
        string Template { get; set; }
        string TemplateStyle { get; set; }
        string TemplateHead { get; set; }
        string TemplateBody { get; set; }
        string LookupColumn { get; set; }
        bool EnableSsl { get; set; }
    }
}
