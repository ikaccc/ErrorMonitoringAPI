using ExceptionHandler.API.Dtos;
using ExceptionHandler.API.Enums;
using ExceptionHandler.API.Models;
using System;
using System.Threading.Tasks;
using Exception = ExceptionHandler.API.Models.Exception;

namespace ExceptionHandler.API.Intefaces
{
    public interface IDataManager
    {
        //get db config data for requested user. User can store data in own db
        //if user has set own db application will store data in both dbs (azure application db and user db)
        Task<ApplicationDbConfig> GetDbConfig(string accessToken);

        //get application settings for requested user
        Task<RegistredApplications> GetRegistredApplication(string accessToken);
        //Task<string> DecriptDbPassword(string dbConfigDbPassword);

        //Proccess methods stored exception data to db
        bool ProcessData(Payload data, ApplicationDbConfig dbConfig);
        Task<Guid> ProccessException(Exception ex);
        Task<AuthenticateResponse> GetUserByUserAndPass(Func<object, bool> p);
        Task<Guid> ProccessTrace(Guid exceptionId);
        Task<Guid> ProccessCodeContext(CodeContext itemContext);
        Task<Guid> ProccessFrames(Guid traceId, Frame item, Guid codeContextId);
        Task<Guid> ProccessCrashReport(CrashReport bodyCrashReport);
        Task<Guid> ProccessMessage(Message bodyMessage);
        Task<Guid> ProccessPerson(Person dataPerson);
        Task<Guid> ProccessBody(Guid traceId, Guid messageId, Guid crashReportId);
        Task<Guid> ProccessServer(Server dataServer);
        Task<Guid> ProccessData(Guid personId, Guid serverId, Guid bodyId, ErrorLevel errorLevel, Payload data);
        Task<Guid> ProccessTraceFrames(Guid traceId, Guid frameId);

        //Get email setting for requested user
        Task<EmailSettingsDto> GetEmailSettings(string accessToken, Guid emailType);

        Task SetMailSendFlag(Guid exceptionId, string errorMessage = null);
        Task<WebUser> GetWebUser(Guid userId);
    }
}
