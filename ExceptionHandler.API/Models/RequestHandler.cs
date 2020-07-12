using ExceptionHandler.API.Enums;
using ExceptionHandler.API.Intefaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExceptionHandler.API.Models
{
    public class RequestHandler : IRequestHandler
    {
        private readonly IDataManager _dataManager;
        private readonly ISecurity _security;
        private readonly IEmailSender _emailSender;

        public RequestHandler(IDataManager dataManager, ISecurity security, IEmailSender emailSender)
        {
            _dataManager = dataManager;
            _security = security;
            _emailSender = emailSender;
        }

        public async Task<bool> ProcessData(Payload data)
        {
            var dbConfig = await _dataManager.GetDbConfig(data.AccessToken);
            var decriptPass = await _security.DecriptDbPassword(dbConfig.DbPassword);
            //TODO: ako se ovozmozi na kraen korisnik da moze da vnese server, baza, user i pass treba na db-layer-ot da mu se prosledi configot i IDataBaseProvider instancata da se reinicijalizira so prosledeniot konfig
            //NOTICE: dekriptioranjeto t.e kriptiranjeto e po nekoja moja luda logika, ali NIKADE vo aplikacija ili baza ne se zapisuva KLUCOT so koj se kriptira stringot. Sekoj vnesen password se dekriptira so random IV KEY

            try
            {
                var exceptionId = Guid.Empty;
                if (data.Data?.Body?.Trace?.Exception != null)
                {
                    exceptionId = await _dataManager.ProccessException(data.Data.Body.Trace.Exception);
                }

                var traceId = Guid.Empty;
                if (data.Data?.Body?.Trace != null)
                {
                    traceId = await _dataManager.ProccessTrace(exceptionId);
                }

                var stackTraceFramesIds = new List<Guid>();
                if (data.Data?.Body?.Trace?.Frames != null && data.Data?.Body?.Trace.Frames.Length > 0)
                {
                    foreach (var item in data.Data?.Body?.Trace?.Frames)
                    {
                        var codeContextId = Guid.Empty;
                        if (item.Context != null)
                        {
                            codeContextId = await _dataManager.ProccessCodeContext(item.Context);
                        }
                        //stackTraceFramesIds.Add(await _dataManager.ProccessFrames(traceId, item, codeContextId));
                        var frameId = await _dataManager.ProccessFrames(traceId, item, codeContextId);
                        await _dataManager.ProccessTraceFrames(traceId, frameId);
                    }
                }


                var crashReportId = Guid.Empty;
                if (data.Data?.Body?.CrashReport != null)
                {
                    crashReportId = await _dataManager.ProccessCrashReport(data.Data.Body.CrashReport);
                }

                var messageId = Guid.Empty;
                if (data.Data?.Body?.Message != null)
                {
                    messageId = await _dataManager.ProccessMessage(data.Data.Body.Message);
                }

                var personId = Guid.Empty;
                if (data.Data?.Person != null)
                {
                    personId = await _dataManager.ProccessPerson(data.Data.Person);
                }

                var bodyId = await _dataManager.ProccessBody(traceId, messageId, crashReportId);

                var serverId = Guid.Empty;
                if (data.Data?.Server != null)
                {
                    serverId = await _dataManager.ProccessServer(data.Data.Server);
                }


                var dataId = await _dataManager.ProccessData(personId, serverId, bodyId, data.Data?.Level ?? ErrorLevel.None, data);

                if (data.Data != null)
                {
                    data.Data.Id = dataId;
                }

                await _emailSender.SendMailProccess(data.AccessToken, data, EmailType.NewItem, await GetRegistredApplication(data.AccessToken));
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        public async Task<RegistredApplications> GetRegistredApplication(string accessToken)
        {
            return await _dataManager.GetRegistredApplication(accessToken);
        }

        public async Task<WebUser> GetWebUser(Guid userId)
        {
            return await _dataManager.GetWebUser(userId);
        }

        //private string GetFrameLine(Frame item)
        //{
        //    return "at " + item.Method + (Assumption.AssertNotNullOrWhiteSpaceBool(item.FileName)
        //               ? " in " + item.FileName
        //               : ((item.Method.Split('.').Length > 3 ? " in " + string.Join(".", item.Method.Split('.').Take(3)) : " in (unknown)")));
        //}
    }
}
