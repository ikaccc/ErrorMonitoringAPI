using System.Threading.Tasks;

namespace ExceptionHandler.API.Intefaces
{
    public interface ISecurity
    {
        Task<bool> CheckTokenAndUser(string dataAccessToken);

        Task<string> DecriptDbPassword(string dbConfigDbPassword);
    }
}
