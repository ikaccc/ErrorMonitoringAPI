using System;
using ExceptionHandler.API.Models;
using System.Threading.Tasks;

namespace ExceptionHandler.API.Intefaces
{
    public interface IRequestHandler
    {
        Task<bool> ProcessData(Payload data);
        Task<RegistredApplications> GetRegistredApplication(string accessToken);
        Task<WebUser> GetWebUser(Guid userId);
    }
}
