﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ReactNet.Models;

namespace ReactNet.Repository
{
    public class FamilyRepositoryInMemory : IFamilyRepository
    {
        private ConcurrentDictionary<string, PersonDb> _peopleDb;
        private readonly IMapper _mapper;

        public FamilyRepositoryInMemory(IMapper mapper, ConcurrentDictionary<string, PersonDb> peopleDb)
        {
            _mapper = mapper;
            _peopleDb = peopleDb;
        }

        public IEnumerable<FamilyTreePerson> GetFamilyTree()
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

        public PersonDetails GetDetails(string id)
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
                    Label = _peopleDb[spouseDb].PreferredName,
                    Type = FamilyType.Spouse
                });
            }

            foreach (var parentDb in personDb.Parents)
            {
                personDetails.Family.Add(new Family
                {
                    Id = parentDb,
                    Label = _peopleDb[parentDb].PreferredName,
                    Type = FamilyType.Parent
                });
            }

            foreach (var childDb in personDb.Children)
            {
                personDetails.Family.Add(new Family
                {
                    Id = childDb,
                    Label = _peopleDb[childDb].PreferredName,
                    Type = FamilyType.Child
                });
            }

            foreach (var siblingDb in personDb.Siblings)
            {
                personDetails.Family.Add(new Family
                {
                    Id = siblingDb,
                    Label = _peopleDb[siblingDb].PreferredName,
                    Type = FamilyType.Sibling
                });
            }

            return personDetails;
        }
    }
}