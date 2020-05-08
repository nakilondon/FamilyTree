using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ReactNet.Models;

namespace ReactNet.Repositories
{
    public class FamilyRepositoryInMemory : IFamilyRepository
    {
        private ConcurrentDictionary<string, PersonOldDb> _peopleDb;
        private readonly IMapper _mapper;

        public FamilyRepositoryInMemory(IMapper mapper, ConcurrentDictionary<string, PersonOldDb> peopleDb)
        {
            _mapper = mapper;
            _peopleDb = peopleDb;
        }

        public async Task<IEnumerable<FamilyTreePerson>> GetFamilyTree()
        {
            var peopleTree = new List<FamilyTreePerson>();

            foreach (var personDb in _peopleDb)
            {
                var personTree = _mapper.Map<FamilyTreePerson>(personDb.Value);
                if (personDb.Value.Gender == Gender.Female)
                {
                    personTree.ItemTitleColor = "#FDD7E4";
                    personTree.Image = "/img/Female.png";
                }

                if (personDb.Value.Gender == Gender.Male)
                {
                    personTree.Image = "/img/Male.png";
                }

                peopleTree.Add(personTree);
            }

     
            foreach (var person in peopleTree)
            {
                if (person.Parents.Any())
                {
                    var siblings = peopleTree.FindAll(p => p.Parents.Contains(person.Parents.First()))
                        .OrderBy(p => p.BirthDate);

                    short position = 0;
                    string relativeId = null;
                    foreach (var sibling in siblings)
                    {
                        if (position == 0)
                        {
                            relativeId = sibling.Id;
                        }
                        else
                        {
                            sibling.RelativeItem = relativeId;
                            sibling.PlacementType = AdviserPlacementType.Right;
                            sibling.Position = position;
                        }

                        position++;
                    }
                }
            }

            return peopleTree;
        }

        public IEnumerable<ListPerson> GetList()
        {
            var peopleList = new List<ListPerson>();
            foreach (var personDb in _peopleDb)
            {
                peopleList.Add(_mapper.Map<ListPerson>(personDb.Value));
            }

            return peopleList;
        }

        public async Task<PersonDetails> GetDetails(string id)
        {
            if (!_peopleDb.ContainsKey(id))
            {
                return null;
            }

            var personDb = _peopleDb[id];
            var personDetails = _mapper.Map<PersonDetails>(personDb);
            personDetails.Family = new List<Family>();

            foreach (var spouseDb in personDb.Spouses)
            {
                personDetails.Family.Add(new Family
                {
                    Id = spouseDb,
                    Name = _peopleDb[spouseDb].PreferredName,
                    Relationship = "Spouse"
                });
            }

            foreach (var parentDb in personDb.Parents)
            {
                personDetails.Family.Add(new Family
                {
                    Id = parentDb,
                    Name = _peopleDb[parentDb].PreferredName,
                    Relationship = "Parent"
                });
            }

            foreach (var childDb in personDb.Children)
            {
                personDetails.Family.Add(new Family
                {
                    Id = childDb,
                    Name = _peopleDb[childDb].PreferredName,
                    Relationship = "Child"
                });
            }

            foreach (var siblingDb in personDb.Siblings)
            {
                personDetails.Family.Add(new Family
                {
                    Id = siblingDb,
                    Name = _peopleDb[siblingDb].PreferredName,
                    Relationship = "Sibling"
                });
            }

            return personDetails;
        }
    }
}
