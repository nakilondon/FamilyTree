using System.Collections.Generic;
using ReactNet.Models;

namespace ReactNet.Repository
{
    public interface IFamilyRepository
    {
        IEnumerable<FamilyTreePerson> GetFamilyTree();
        IEnumerable<ListPerson> GetList();
        PersonDetails GetDetails(string id);
    }
}
