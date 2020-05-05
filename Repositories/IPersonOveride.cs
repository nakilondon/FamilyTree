using System.Threading.Tasks;
using ReactNet.Models;

namespace ReactNet.Repositories
{
    public interface IPersonOverride
    {
        Task AddPerson(PersonOverride personOverride);
        Task UpdatePerson(PersonOverride personOverride);
        Task<PersonOverride> GetPerson(string id);
    }
}
