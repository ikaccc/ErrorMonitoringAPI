using ExceptionHandler.API.Common;
using ExceptionHandler.API.Dtos;
using ExceptionHandler.API.Enums;
using ExceptionHandler.API.Intefaces;
using ExceptionHandler.API.Models;
using ExceptionHandler.DataAccess;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Exception = ExceptionHandler.API.Models.Exception;

namespace ExceptionHandler.API.DataManager
{
    public class DataManager : IDataManager
    {
        private readonly IConfiguration _config;
        private readonly IDataBaseProvider _dataBase;

        public DataManager(IConfiguration config)
        {
            _config = config;

            //API Connection string
            _dataBase = SqlAccessLayer.CreateDatabaseProvider(_config.GetConnectionString("ExceptionHandlerAPIConnectionString"));
        }

        public async Task<ApplicationDbConfig> GetDbConfig(string accessToken)
        {
            const string sqlStatement = "SELECT DbServer, DbName, DbUserName, DbPassword " +
                                           " FROM RegistredApplications " +
                                           " WHERE AccessToken = @accessToken";

            try
            {
                _dataBase.OpenConnection();

                var res = await _dataBase
                    .GetDataAsync<ApplicationDbConfig>(sqlStatement,
                        _dataBase.GetParameter("@accessToken", DbType.String, accessToken));
                return res.FirstOrDefault();

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<RegistredApplications> GetRegistredApplication(string accessToken)
        {
            const string sqlStatement = " SELECT Id, UserId, AccessToken, IsTrial, DateRegistered, DateUpdated, " +
                               " TrialDuration, ConcurrencyStamp, ApplicationName, DbServer, DbName, " +
                               " DbUserName, DbPassword " +
                               " FROM RegistredApplications " +
                               " WHERE AccessToken = @accessToken";

            try
            {
                _dataBase.OpenConnection();

                var res = await _dataBase
                    .GetDataAsync(sqlStatement,
                        _dataBase.GetParameter("@accessToken", DbType.String, accessToken));

                return res.Select(x => new RegistredApplications
                {
                    Id = x.ReadGuid<Guid>("Id"),
                    UserId = x.ReadGuid<Guid>("UserId"),
                    AccessToken = x.Read<string>("AccessToken"),
                    IsTrial = Convert.ToInt32(x.Read<int>("IsTrial")) == 1,
                    DateRegistred = x.Read<DateTime>("DateRegistered"),
                    DateUpdated = x.Read<DateTime>("DateUpdated"),
                    TrialDuration = x.Read<int>("TrialDuration"),
                    ConcurrencyStamp = x.ReadGuid<Guid>("ConcurrencyStamp"),
                    ApplicationName = x.Read<string>("ApplicationName"),
                    DbServer = x.Read<string>("DbServer"),
                    DbName = x.Read<string>("DbName"),
                    DbUserName = x.Read<string>("DbUserName"),
                    DbPassword = x.Read<string>("DbPassword"),

                }).FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public void InsertEncriptedPassword(string encriptedPass)
        {
            //const string sqlStatement = @"
            //        INSERT  
            //        FROM PostCategory 
            //        WHERE PostId = @postId";

            //var db = _dataManager.DataBase;
            //try
            //{
            //    db.OpenConnection();
            //    db.ExecuteNonQuery(sqlStatement, db.GetParameter("@postId", DbType.String, postId));
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}
        }

        public bool ProcessData(Payload data, ApplicationDbConfig dbConfig)
        {
            //IDataBaseProvider _db;
            //_db = SqlAccessLayer.CreateDatabaseProvider(_config.GetConnectionString("ExceptionHandlerAPIConnectionString"));

            //const string sql = " SELECT Id, UserId, AccessToken, IsTrial, DateRegistered, DateUpdated, " +
            //                   " TrialDuration, ConcurrencyStamp, ApplicationName, DbServer, DbName, " +
            //                   " DbUserName, DbPassword " +
            //                   " FROM RegistredApplications " +
            //                   " WHERE AccessToken = @accessToken";

            //try
            //{
            //    _dataBase.OpenConnection();

            //    var res = await _dataBase
            //        .GetDataAsync(sql,
            //            _dataBase.GetParameter("@accessToken", DbType.String, accessToken));
            //    return res.Select(x => new RegistredApplications
            //    {
            //        Id = new Guid(x.Read<string>("Id")),
            //        UserId = new Guid(x.Read<string>("UserId")),
            //        AccessToken = x.Read<string>("AccessToken"),
            //        IsTrial = Convert.ToInt32(x.Read<int>("AccessToken")) == 1,
            //        DateRegistred = x.Read<DateTime>("DateRegistred"),
            //        DateUpdated = x.Read<DateTime>("DateUpdated"),
            //        TrialDuration = x.Read<int>("TrialDuration"),
            //        ConcurrencyStamp = new Guid(x.Read<string>("ConcurrencyStamp")),
            //        ApplicationName = x.Read<string>("ApplicationName"),
            //        DbServer = x.Read<string>("DbServer"),
            //        DbName = x.Read<string>("DbName"),
            //        DbUserName = x.Read<string>("DbUserName"),
            //        DbPassword = x.Read<string>("DbPassword"),

            //    }).FirstOrDefault();
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    _dataBase.CloseConnection();
            //}
            return true;
        }

        public async Task<Guid> ProccessException(Exception exc)
        {
            const string storedProcedureName = "[dbo].[ProcessException]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName, _dataBase.GetParameter("@class", DbType.String, exc.Class),
                                                                                     _dataBase.GetParameter("@message", DbType.String, exc.Message),
                                                                                     _dataBase.GetParameter("@description", DbType.String, exc.Description),
                                                                                    _dataBase.GetParameter("@stackTrace", DbType.String, exc.StackTrace));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<Guid> ProccessTrace(Guid exceptionId)
        {
            const string storedProcedureName = "[dbo].[ProcessTrace]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName, _dataBase.GetParameter("@exceptionId", DbType.Guid, exceptionId.OptionalParam()));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<Guid> ProccessCodeContext(CodeContext codeContext)
        {
            const string storedProcedureName = "[dbo].[ProccessCodeContext]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName, _dataBase.GetParameter("@pre", DbType.String, string.Join(",", codeContext.Pre)),
                    _dataBase.GetParameter("@post", DbType.String, string.Join(",", codeContext.Post)));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<Guid> ProccessFrames(Guid traceId, Frame item, Guid codeContextId)
        {
            const string storedProcedureName = "[dbo].[ProccessFrames]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName,
                    _dataBase.GetParameter("@codeContextId", DbType.Guid, codeContextId.OptionalParam()),
                    _dataBase.GetParameter("@traceId", DbType.Guid, traceId.OptionalParam()),
                    _dataBase.GetParameter("@args", DbType.String, item.Args != null ? string.Join(",", item.Args) : string.Empty),
                    _dataBase.GetParameter("@code", DbType.String, item.Code),
                    _dataBase.GetParameter("@colNo", DbType.Int64, item.ColNo),
                    _dataBase.GetParameter("@fileName", DbType.String, item.FileName),
                    _dataBase.GetParameter("@lineNo", DbType.Int64, item.LineNo),
                    _dataBase.GetParameter("@method", DbType.String, item.Method));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<Guid> ProccessCrashReport(CrashReport bodyCrashReport)
        {
            const string storedProcedureName = "[dbo].[ProcessCrashReport]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName, _dataBase.GetParameter("@raw", DbType.String, bodyCrashReport.Raw));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<Guid> ProccessMessage(Message bodyMessage)
        {
            const string storedProcedureName = "[dbo].[ProccessMessage]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName, _dataBase.GetParameter("@body", DbType.String, bodyMessage.Body));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<Guid> ProccessPerson(Person bodyPerson)
        {
            const string storedProcedureName = "[dbo].[ProccessPerson]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName,
                    _dataBase.GetParameter("@email", DbType.String, bodyPerson.Email),
                    _dataBase.GetParameter("@userName", DbType.String, bodyPerson.UserName),
                    _dataBase.GetParameter("@userAgent", DbType.String, bodyPerson.UserAgent),
                    _dataBase.GetParameter("@ipAddress", DbType.String, bodyPerson.IpAddress),
                    _dataBase.GetParameter("@userId", DbType.String, bodyPerson.Id));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<Guid> ProccessBody(Guid traceId, Guid messageId, Guid crashReportId)
        {
            const string storedProcedureName = "[dbo].[ProccessBody]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName, _dataBase.GetParameter("@traceId", DbType.Guid, traceId.OptionalParam()),
                    _dataBase.GetParameter("@messageId", DbType.Guid, messageId.OptionalParam()),
                    _dataBase.GetParameter("@crashReportId", DbType.Guid, crashReportId.OptionalParam()));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<Guid> ProccessServer(Server dataServer)
        {
            const string storedProcedureName = "[dbo].[ProccessServer]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName, _dataBase.GetParameter("@branch", DbType.String, dataServer.Branch),
                    _dataBase.GetParameter("@codeVersion", DbType.String, dataServer.CodeVersion),
                    _dataBase.GetParameter("@host", DbType.String, dataServer.Host),
                    _dataBase.GetParameter("@root", DbType.String, dataServer.Root));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<Guid> ProccessData(Guid personId, Guid serverId, Guid bodyId, ErrorLevel errorLevel, Payload data)
        {
            const string storedProcedureName = "[dbo].[ProccessData]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName,
                    _dataBase.GetParameter("@personId", DbType.Guid, personId.OptionalParam()),
                    _dataBase.GetParameter("@serverId", DbType.Guid, serverId.OptionalParam()),
                    _dataBase.GetParameter("@bodyId", DbType.Guid, bodyId.OptionalParam()),
                    _dataBase.GetParameter("@errorLevel", DbType.String, errorLevel.ToString()),
                    _dataBase.GetParameter("@codeVersion", DbType.String, data.Data.CodeVersion),
                    _dataBase.GetParameter("@context", DbType.String, data.Data.Context),
                    _dataBase.GetParameter("@environment", DbType.String, data.Data.Environment),
                    _dataBase.GetParameter("@fingerPrint", DbType.String, data.Data.Fingerprint),
                    _dataBase.GetParameter("@framework", DbType.String, data.Data.Framework),
                    _dataBase.GetParameter("@language", DbType.String, data.Data.Language),
                    _dataBase.GetParameter("@timestamp", DbType.Int64, data.Data.Timestamp.OptionalParam()),
                    _dataBase.GetParameter("@title", DbType.String, data.Data.Title),
                    _dataBase.GetParameter("@platform", DbType.String, data.Data.Platform),
                    _dataBase.GetParameter("@uuid", DbType.String, data.Data.Uuid));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<Guid> ProccessTraceFrames(Guid traceId, Guid frameId)
        {
            const string storedProcedureName = "[dbo].[ProccessTraceFrames]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithScalarAsync<string>(storedProcedureName,
                    _dataBase.GetParameter("@traceId", DbType.Guid, traceId.OptionalParam()),
                    _dataBase.GetParameter("@frameId", DbType.Guid, frameId.OptionalParam()));

                return !string.IsNullOrEmpty(res) && !string.IsNullOrWhiteSpace(res) ? new Guid(res) : Guid.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<EmailSettingsDto> GetEmailSettings(string accessToken, Guid emailType)
        {
            const string storedProcedureName = "[dbo].[GetEmailSettings]";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteProcedureWithResultAsync(storedProcedureName,
                    _dataBase.GetParameter("@accessToken", accessToken));
                return res.Select(x => new EmailSettingsDto
                {
                    RegistredApplicationId = x.ReadGuid<Guid>("RegistredApplicationId"),
                    FromEmailAddress = x.Read<string>("FromEmailAddress"),
                    FromName = x.Read<string>("FromName"),
                    ToEmailAddress = x.Read<string>("ToEmailAddresses"),
                    ToNames = x.Read<string>("ToNames"),
                    CcEmailAddresses = x.Read<string>("CCEmailAddresses"),
                    CcNames = x.Read<string>("CCNames"),
                    BCcEmailAddresses = x.Read<string>("BCCEmailAddresses"),
                    BCcNames = x.Read<string>("BCCNames"),
                    EmailSubject = x.Read<string>("EmailSubject"),
                    IsHtml = x.Read<int>("IsHtml") == 1,
                    AddAttachment = x.Read<int>("AddAttachment") == 1,
                    SMTPPassword = x.Read<string>("SMTPPassword"),
                    SMTPPort = x.Read<int>("SMTPPort"),
                    SMTPServer = x.Read<string>("SMTPServer"),
                    SMTPUserName = x.Read<string>("SMTPUserName"),
                    Template = x.Read<string>("Template"),
                    TemplateStyle = x.Read<string>("TemplateStyle"),
                    TemplateHead = x.Read<string>("TemplateHead"),
                    TemplateBody = x.Read<string>("TemplateBody"),
                    LookupColumn = x.Read<string>("LookupColumn"),
                    TitleTemplate = x.Read<string>("TitleTemplate"),
                    EnableSsl = x.Read<int>("EnableSsl") == 1
                }).SingleOrDefault();

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task SetMailSendFlag(Guid exceptionId, string errorMessage)
        {
            const string sqlStatement = "UPDATE Data SET IsMailSend = 1 WHERE Id = @exceptionId";

            try
            {
                _dataBase.OpenConnection();
                var res = await _dataBase.ExecuteNonQueryAsync(sqlStatement,
                    _dataBase.GetParameter("@exceptionId", exceptionId));
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }

        public async Task<WebUser> GetWebUser(Guid userId)
        {
            const string sqlStatement = " SELECT Id, FullName, TimeZoneId, SendAlerts, Email, AdditionalEmail " +
                                        " FROM WebUser " +
                                        " WHERE Id = @userId";

            try
            {
                _dataBase.OpenConnection();

                var res = await _dataBase
                    .GetDataAsync(sqlStatement,
                        _dataBase.GetParameter("@userId", DbType.Guid, userId));

                return res.Select(x => new WebUser
                {
                    UserId = x.ReadGuid<Guid>("Id"),
                    AdditionalEmail = x.ReadGuid<string>("AdditionalEmail"),
                    Email = x.ReadGuid<string>("Email"),
                    FullName = x.ReadGuid<string>("FullName"),
                    TimeZoneId = x.ReadGuid<string>("TimeZoneId"),
                    SendAlerts = x.ReadGuid<int>("SendAlerts") == 1
                }).FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataBase.CloseConnection();
            }
        }
    }
}
