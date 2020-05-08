using System.Threading.Tasks;

namespace ReactNet.Repositories
{
    public interface IPersonRepository
    {
        Task AddPerson(PersonDb personDb);
        Task<PersonDb> FindPerson(string gedcomId);
        Task AddRelationship(RelationshipDb relationshipDb);
    }
}
