using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeneGenie.Gedcom;
using GeneGenie.Gedcom.Enums;
using GeneGenie.Gedcom.Parser;
using Microsoft.Extensions.Configuration;


namespace ReactNet.Repositories
{
    public class Gedcom : IGedcom
    {
        private readonly IConfiguration _configuration;
        private readonly IPersonRepository _personRepository;

        public Gedcom(IConfiguration configuration, IPersonRepository personRepository)
        {
            _configuration = configuration;
            _personRepository = personRepository;
        }

        private EventDate EventDateFromGed(GedcomDate gedcomDate)
        {
            var eventDate = new EventDate();

            if (gedcomDate != null && gedcomDate.DateTime1 != null)
            {
                eventDate.Date = (DateTime) gedcomDate.DateTime1;
                eventDate.Certain = gedcomDate.DatePeriod == GedcomDatePeriod.Exact;
            }

            return eventDate;
        }

        private string NameFromGed(GedcomName gedcomName)
        {
            string name;

            var position = gedcomName.Given.IndexOf(' ');
            if (position > 0)
            {
                name = gedcomName.Given.Substring(0, gedcomName.Given.IndexOf(' ')) + " " + gedcomName.Surname;
            }
            else
            {
                name = gedcomName.Given + " " + gedcomName.Surname;
            }

            return name;
        }

        public async Task<ConcurrentDictionary<string, PersonDb>> CreatePersonDbFromGedcom()
        {
            var gedcomFile = _configuration.GetSection("GedcomFIle").Value;
            var personDictionary = new ConcurrentDictionary<string, PersonDb>();
            var gedcomRecordReader = GedcomRecordReader.CreateReader(gedcomFile);
            if (gedcomRecordReader.Parser.ErrorState != GedcomErrorState.NoError)
            {
                Console.WriteLine($"Could not read file, encountered error {gedcomRecordReader.Parser.ErrorState}.");
            }

            var gedcomDb = gedcomRecordReader.Database;

            foreach (var gedcomDbIndividual in gedcomDb.Individuals)
            {
                var relationships = new List<RelationshipTable>();
                var gedName = gedcomDbIndividual.GetName();

                var personDb = new PersonDb
                {
                    GedcomId = gedcomDbIndividual.XRefID,
                    Gender = gedcomDbIndividual.Sex == GedcomSex.Female ? Gender.Female.ToString() : Gender.Male.ToString(),
                    PreferredName = NameFromGed(gedName),
                    Surname = gedName.Surname,
                    GivenNames = gedName.Given,
                    Dead = gedcomDbIndividual.Dead
                };

                if (gedcomDbIndividual.Birth != null)
                {
                    if (gedcomDbIndividual.Birth?.Date != null)
                    {
                        if (gedcomDbIndividual.Birth.Date?.DateTime1 != null)
                            personDb.BirthRangeStart = (DateTime) gedcomDbIndividual.Birth.Date.DateTime1;
                        if (gedcomDbIndividual.Birth.Date?.DateTime2 != null)
                            personDb.BirthRangeEnd = (DateTime) gedcomDbIndividual.Birth.Date.DateTime2;
                    }

                    if (gedcomDbIndividual.Birth?.Place?.Name != null)
                        personDb.PlaceOfBirth = gedcomDbIndividual.Birth.Place.Name;
                }

                if (gedcomDbIndividual.Death != null)
                {
                    if (gedcomDbIndividual.Death?.Date != null)
                    {
                        if (gedcomDbIndividual.Death.Date?.DateTime1 != null)
                            personDb.DeathRangeStart = (DateTime) gedcomDbIndividual.Death.Date.DateTime1;
                        if (gedcomDbIndividual.Death.Date?.DateTime2 != null)
                            personDb.DeathRangeEnd = (DateTime) gedcomDbIndividual.Death.Date.DateTime2;
                    }

                    if (gedcomDbIndividual.Death?.Place?.Name != null)
                    {
                        personDb.PlaceOfDeath = gedcomDbIndividual.Death.Place.Name;
                    }
                }

                var familyRecords =
                    gedcomDb.Families.FindAll(f => f.Husband == gedcomDbIndividual.XRefID || f.Wife == gedcomDbIndividual.XRefID);

                foreach (var familyRecord in familyRecords)
                {

                    string spouseId = null;
                    var relationship = Relationship.Spouse;

                    var wife = familyRecord.Wife;
                    if (wife != null && wife != gedcomDbIndividual.XRefID)
                    {
                        relationship = Relationship.Wife;
                        spouseId = wife;
                    }
                    else
                    {
                        var husband = familyRecord.Husband;
                        if (husband != null && husband != gedcomDbIndividual.XRefID)
                        {
                            relationship = Relationship.Husband;
                            spouseId = husband;
                        }
                    }

                    if (!string.IsNullOrEmpty(spouseId))
                    {
                        relationships.Add(new RelationshipTable
                        {
                            PersonGedcomId = spouseId,
                            Relationship = relationship
                        });
                    }

                    foreach (var child in familyRecord.Children)
                    {
                        relationships.Add(new RelationshipTable
                        {
                            Relationship = gedcomDb.Individuals.Find(i => i.XRefID == child)?.Sex switch
                            {
                                GedcomSex.Female => Relationship.Daughter,
                                GedcomSex.Male => Relationship.Son,
                                _ => Relationship.Child
                            },
                            PersonGedcomId = child,
                        });
                    }
                }

                foreach (var family in gedcomDbIndividual.ChildIn)
                {
                    var parentsDb = gedcomDb
                        .Individuals.FindAll((i => i.SpouseIn != null && i.SpouseInFamily(family.Family)));

                    foreach (var parentDb in parentsDb)
                    {
                        if (!parentDb.Equals(gedcomDbIndividual))
                        {
                            relationships.Add(new RelationshipTable
                            {
                                Relationship = parentDb.Sex switch
                                {
                                    GedcomSex.Female => Relationship.Mother,
                                    GedcomSex.Male => Relationship.Father,
                                    _ => Relationship.Parent
                                },
                                PersonGedcomId = parentDb.XRefID
                            });
                        }
                    }
                }

                var siblingsFamilyRecords =
                    gedcomDb.Families.FindAll(f => relationships.Find(
                        fd => fd.Relationship == Relationship.Father && fd.PersonGedcomId == f.Husband ||
                              fd.Relationship == Relationship.Mother && fd.PersonGedcomId == f.Wife) != null);

                foreach (var siblingsFamily in siblingsFamilyRecords)
                {
                    var siblings = gedcomDb.Individuals.FindAll(i => siblingsFamily.Children.Contains(i.XRefID))
                        .OrderBy(i => i.Birth?.Date);

                    foreach (var sibling in siblings)
                    {
                        if (!sibling.Equals(gedcomDbIndividual))
                        {
                            relationships.Add(new RelationshipTable
                            {
                                Relationship = sibling.Sex switch
                                {
                                    GedcomSex.Female => Relationship.Sister,
                                    GedcomSex.Male => Relationship.Brother,
                                    _ => Relationship.Sibling
                                },
                                PersonGedcomId = sibling.XRefID
                            });
                        }
                    }
                }

                personDb.Relationships = relationships;


               // await _personRepository.AddPerson(personDb);
                personDictionary.TryAdd(personDb.GedcomId, personDb);
                try
                {
                    var returnedPerson = await _personRepository.FindPerson(personDb.GedcomId);
                    foreach (var relationship in relationships)
                    {
                        var relationshipPerson = await _personRepository.FindPerson(relationship.PersonGedcomId);
                        var relationshipDb = new RelationshipDb
                        {
                            Person1 = returnedPerson.Id,
                            Person2 = relationshipPerson.Id,
                            RelationShip = relationship.Relationship.ToString()
                        };
                        await _personRepository.AddRelationship(relationshipDb);

                    }
                }
                catch (Exception e)
                { }
            }

            return personDictionary;
        }
    }

}