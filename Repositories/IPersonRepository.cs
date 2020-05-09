using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactNet.Repositories
{
    public interface IPersonRepository
    {
        Task AddPerson(PersonDb personDb);
        Task<PersonDb> FindPerson(int id);
        Task AddRelationship(RelationshipDb relationshipDb);
        Task<IDictionary<int, PersonDb>> FindAllPeople();
    }
}
