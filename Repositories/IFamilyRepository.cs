using System.Collections.Generic;
using System.Threading.Tasks;
using ReactNet.Models;

namespace ReactNet.Repositories
{
    public interface IFamilyRepository
    {
        Task<IEnumerable<FamilyTreePerson>> GetFamilyTree();
        IEnumerable<ListPerson> GetList();
        Task<PersonDetails> GetDetails(string id);
    }
}
