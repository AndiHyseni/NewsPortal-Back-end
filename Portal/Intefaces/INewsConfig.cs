using Portal.Models;
using System.Threading.Tasks;

namespace Portal.Intefaces
{
    public interface INewsConfig
    {
        Task<NewsConfig> GetConfigRow();
        Task EditConfigRow(NewsConfig config);
    }
}
