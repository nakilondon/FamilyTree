using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeneGenie.Gedcom;
using GeneGenie.Gedcom.Enums;
using GeneGenie.Gedcom.Parser;
using Microsoft.Extensions.Configuration;
using ReactNet.Models;

namespace ReactNet.Repositories
{
    public class GedFamilyRepository : IFamilyRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IPersonOverride _personOverrideDb;
        private readonly IMapper _mapper;

        public GedFamilyRepository(IConfiguration configuration, IPersonOverride personOverrideDb, IMapper mapper)
        {
            _configuration = configuration;
            _personOverrideDb = personOverrideDb;
            _mapper = mapper;
        }

        public async Task<string> NameFromGed(string id, GedcomName gedcomName)
        {

            try
            {
                var overridePerson = await _personOverrideDb.GetPerson(id);
                if (overridePerson?.PreferredName != null)
                    return overridePerson.PreferredName;
            }
            catch (Exception e)
            {
            }

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
    

        private GedcomDatabase _gedcomDb;
        
        private GedcomDatabase GedcomDb
        {
            get
            {
                if (_gedcomDb == null)
                {
                    var gedcomRecordReader = GedcomRecordReader.CreateReader(_configuration.GetSection("GedcomFIle").Value);
                    if (gedcomRecordReader.Parser.ErrorState != GedcomErrorState.NoError)
                    {
                        Console.WriteLine($"Could not read file, encountered error {gedcomRecordReader.Parser.ErrorState}.");
                    }

                    _gedcomDb = gedcomRecordReader.Database;
                }

                return _gedcomDb;
            }
        }

        public async Task<IEnumerable<FamilyTreePerson>> GetFamilyTree()
        {
            var familyTreePeople = new List<FamilyTreePerson>();

            foreach (var gdIndividual in GedcomDb.Individuals)
            {
                var familyTreePerson = new FamilyTreePerson
                {
                    Id = gdIndividual.XRefID,
                    Title = await NameFromGed(gdIndividual.XRefID, gdIndividual.GetName())
                };

                if (gdIndividual.Birth != null || gdIndividual.Death != null)
                {
                    familyTreePerson.Description = "(";
                    if (gdIndividual.Birth?.Date?.DateTime1 != null)
                    {
                        familyTreePerson.BirthDate = (DateTime)gdIndividual.Birth.Date.DateTime1;
                        familyTreePerson.Description += gdIndividual.Birth.Date.DateString;
                    }


                    if (gdIndividual.Death?.Date?.DateTime1 != null)
                    {
                        familyTreePerson.Description += " - " + gdIndividual.Death.Date.DateString;
                    }

                    familyTreePerson.Description += ")";
                }

                if (gdIndividual.Sex == GedcomSex.Female)
                {
                    familyTreePerson.ItemTitleColor = "#FDD7E4";
                    familyTreePerson.Image = "/img/Female.png";
                }

                if (gdIndividual.Sex == GedcomSex.Male)
                {
                    familyTreePerson.Image = "/img/Male.png";
                }

                var overidePerson = await _personOverrideDb.GetPerson(gdIndividual.XRefID);

                if (!string.IsNullOrWhiteSpace(overidePerson?.Portrait))
                {
                    familyTreePerson.Image = $"familytree/thumbnail/{overidePerson.Portrait}";
                }

                var familyRecord = gdIndividual.GetFamily();
                if (familyRecord != null)
                {
                    var wife = familyRecord.Wife;
                    if (wife != null && wife != gdIndividual.XRefID)
                    {
                        familyTreePerson.Spouses = new List<string> { wife };
                    }

                    var husband = familyRecord.Husband;
                    if (husband != null && husband != gdIndividual.XRefID)
                    {
                        familyTreePerson.Spouses = new List<string> { husband };
                    }
                }

                var parents = new List<string>();
                foreach (var family in gdIndividual.ChildIn)
                {
                    var parentsDb = GedcomDb
                        .Individuals.FindAll((i => i.SpouseIn != null && i.SpouseInFamily(family.Family)));
                    foreach (var parentDb in parentsDb)
                    {
                        if (!parentDb.Equals(gdIndividual))
                        {
                            parents.Add(parentDb.XRefID);
                        }
                    }
                }

                familyTreePerson.Parents = parents;
                familyTreePeople.Add(familyTreePerson);
            }

            foreach (var person in familyTreePeople)
            {
                if (person.Parents.Any())
                {
                    var siblings = familyTreePeople.FindAll(p => p.Parents.Contains(person.Parents.First()))
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
            return familyTreePeople;
        }

        public IEnumerable<ListPerson> GetList()
        {
            var listPeople = new List<ListPerson>();
            foreach (var gdIndividual in GedcomDb.Individuals)
            {
                var label = NameFromGed(gdIndividual.XRefID, gdIndividual.GetName()).Result;
               
                if (gdIndividual.Birth != null || gdIndividual.Death != null)
                {
                    label += " (";
                    if (gdIndividual.Birth != null && gdIndividual.Birth.Date != null)
                    {
                        label += gdIndividual.Birth.Date.DateString;
                    }

                    if (gdIndividual.Death != null && gdIndividual.Death.Date != null)
                    {

                        label += " - " + gdIndividual.Death.Date.DateString;
                    }

                    label += ")";
                }
                var person = new ListPerson
                {
                    Id = gdIndividual.XRefID,
                    Label = label
                };
                
                listPeople.Add(person);
            }

            return listPeople;
        }

        public async Task<PersonDetails> GetDetails(string id)
        {
            var gedPerson = GedcomDb.Individuals.First(i => i.XRefID == id);


            var familyDetails = new List<Family>();


            var familyRecords =
                GedcomDb.Families.FindAll(f => f.Husband == gedPerson.XRefID || f.Wife == gedPerson.XRefID);

            foreach (var familyRecord in familyRecords)
            {

                string spouseId = null;
                string relationship = null;

                var wife = familyRecord.Wife;
                if (wife != null && wife != gedPerson.XRefID)
                {
                    relationship = "Wife";
                    spouseId = wife;
                }
                else
                {
                    var husband = familyRecord.Husband;
                    if (husband != null && husband != gedPerson.XRefID)
                    {
                        relationship = "Husband";
                        spouseId = husband;
                    }
                }

                if (!string.IsNullOrEmpty(spouseId))
                {
                    familyDetails.Add(new Family
                    {
                        Relationship = relationship ?? "Spouse",
                        Id = spouseId,
                        Name = await NameFromGed(spouseId, GedcomDb.Individuals.Find(i => i.XRefID == spouseId)?.GetName())
                    });
                }

                foreach (var child in familyRecord.Children)
                {
                    familyDetails.Add(new Family
                    {
                        Relationship = GedcomDb.Individuals.Find(i => i.XRefID == child)?.Sex switch
                        {
                            GedcomSex.Female => "Daughter",
                            GedcomSex.Male => "Son",
                            _ => "Child"
                        },
                        Id = child,
                        Name = await NameFromGed(child, GedcomDb.Individuals.Find(i => i.XRefID == child)?.GetName())
                    });
                }
            }

            foreach (var family in gedPerson.ChildIn)
            {
                var parentsDb = GedcomDb
                    .Individuals.FindAll((i => i.SpouseIn != null && i.SpouseInFamily(family.Family)));

                foreach (var parentDb in parentsDb)
                {
                    if (!parentDb.Equals(gedPerson))
                    {
                        familyDetails.Add(new Family
                        {
                            Relationship = parentDb.Sex switch
                            {
                                GedcomSex.Female => "Mother",
                                GedcomSex.Male => "Father",
                                _ => "Parent"
                            },
                            Id = parentDb.XRefID,
                            Name = await NameFromGed(parentDb.XRefID, parentDb.GetName())
                        });
                    }
                }
            }
            
            var siblingsFamilyRecords =
                GedcomDb.Families.FindAll(f => familyDetails.Find(
                    fd => fd.Relationship == "Father" && fd.Id == f.Husband || fd.Relationship == "Mother" && fd.Id == f.Wife) != null);

            foreach (var siblingsFamily in siblingsFamilyRecords)
            {
                var siblings = GedcomDb.Individuals.FindAll(i => siblingsFamily.Children.Contains(i.XRefID))
                    .OrderBy(i => i.Birth?.Date);

                foreach (var sibling in siblings)
                {
                    if (!sibling.Equals(gedPerson))
                    {
                        familyDetails.Add(new Family
                        {
                            Relationship = sibling.Sex switch
                            {
                                GedcomSex.Female => "Sister",
                                GedcomSex.Male => "Brother",
                                _ => "Sibling"
                            },
                            Id = sibling.XRefID,
                            Name = await NameFromGed(sibling.XRefID, sibling.GetName())
                        });
                    }
                }
            }

            var eventList = new List<PersonEvent>();

            eventList.Add(new PersonEvent
            {
                Id = Guid.NewGuid().ToString(),
                Detail = gedPerson.XRefID,
                Place = "XREF"
            });

            foreach (var gedEvent in gedPerson.Events.OrderBy(e => e.Date))
            {
                if (gedEvent.Date != null)
                {
                    eventList.Add(new PersonEvent
                    {
                        Id = Guid.NewGuid().ToString(),
                        EventDate = gedEvent.Date?.DateString,
                        Place = gedEvent.Place?.Name,
                        Detail = gedEvent.EventType switch
                        {
                            GedcomEventType.Birth => "Birth",
                            GedcomEventType.DEAT => "Death",
                            _ => gedEvent.Classification ?? gedEvent.EventName
                        }
                    });
                }
            }
           
            var personDetails = new PersonDetails
            {
                Id = gedPerson.XRefID,
                PreferredName = await NameFromGed(gedPerson.XRefID, gedPerson.GetName()),
                Family = familyDetails,
                Events = eventList,
                Birth = gedPerson.Birth?.Date?.DateString,
                Death = gedPerson.Death?.Date?.DateString,
                FullName = gedPerson.GetName().Name,
                Portrait = gedPerson.Sex switch
                {
                    GedcomSex.Female => "Female.png",
                    _ => "Male.png"
                }
            };


            try
            {
                var overridePerson = await _personOverrideDb.GetPerson(id);
                if (overridePerson != null)
                {
                    if (!string.IsNullOrWhiteSpace(overridePerson.FullName))
                    {
                        personDetails.FullName = overridePerson.FullName;
                    }
                    if (!string.IsNullOrWhiteSpace(overridePerson.Birth))
                    {
                        personDetails.Birth = overridePerson.Birth;
                    }
                    if (!string.IsNullOrWhiteSpace(overridePerson.Death))
                    {
                        personDetails.Death = overridePerson.Death;
                    }
                    if (!string.IsNullOrWhiteSpace(overridePerson.Note))
                    {
                        personDetails.Note = overridePerson.Note;
                    }
                    if (!string.IsNullOrWhiteSpace(overridePerson.Portrait))
                    {
                        personDetails.Portrait = overridePerson.Portrait;
                    }
                }
            }
            catch (Exception e)
            {
            }

            return personDetails;
        }
    }
}
