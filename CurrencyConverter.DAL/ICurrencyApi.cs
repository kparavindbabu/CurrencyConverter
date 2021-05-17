using System.Threading.Tasks;

namespace CurrencyConverter.DAL
{
    public interface ICurrencyApi
    {
        Task<string> Call(string apiUrl);
    }
}