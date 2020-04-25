using System;
using System.Collections.Generic;
using System.Linq;
using GeneGenie.Gedcom;
using GeneGenie.Gedcom.Enums;
using GeneGenie.Gedcom.Parser;
using ReactNet.Models;

namespace ReactNet.Repository
{
    public class GedFamilyRepository : IFamilyRepository
    {
        private readonly string _gedcomFile;

        public GedFamilyRepository(string gedcomFile)
        {
            _gedcomFile = gedcomFile;
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

        private GedcomDatabase _gedcomDb;
        
        private GedcomDatabase GedcomDb
        {
            get
            {
                if (_gedcomDb == null)
                {
                    var gedcomRecordReader = GedcomRecordReader.CreateReader(_gedcomFile);
                    if (gedcomRecordReader.Parser.ErrorState != GedcomErrorState.NoError)
                    {
                        Console.WriteLine($"Could not read file, encountered error {gedcomRecordReader.Parser.ErrorState}.");
                    }

                    _gedcomDb = gedcomRecordReader.Database;
                }

                return _gedcomDb;
            }
        }

        public IEnumerable<FamilyTreePerson> GetFamilyTree()
        {
            var familyTreePeople = new List<FamilyTreePerson>();

            foreach (var gdIndividual in GedcomDb.Individuals)
            {
                var familyTreePerson = new FamilyTreePerson
                {
                    Id = gdIndividual.XRefID,
                    Title = NameFromGed(gdIndividual.GetName())
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
                var label = NameFromGed(gdIndividual.GetName());
               
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

        public PersonDetails GetDetails(string id)
        {
            var gedPerson = GedcomDb.Individuals.First(i => i.XRefID == id);


            var familyDetails = new List<Family>();

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
                            Type = FamilyType.Parent,
                            Id = parentDb.XRefID,
                            Label = NameFromGed(parentDb.GetName())
                        });
                    }
                }

                var siblings = GedcomDb
                    .Individuals.FindAll(i => i.ChildIn != null && i.ChildInFamily(family.Family));
                foreach (var sibling in siblings)
                {
                    if (!sibling.Equals(gedPerson))
                    {
                        familyDetails.Add(new Family
                        {
                            Type = FamilyType.Sibling,
                            Id = sibling.XRefID,
                            Label = NameFromGed(sibling.GetName())
                        });
                    }
                }
            }

            var familyRecords =
                GedcomDb.Families.FindAll(f => f.Husband == gedPerson.XRefID || f.Wife == gedPerson.XRefID);
            foreach (var familyRecord in familyRecords)
            {
             
                string spouseId = null;
                var wife = familyRecord.Wife;
                if (wife != null && wife != gedPerson.XRefID)
                {
                    spouseId = wife;
                }
                else
                {
                    var husband = familyRecord.Husband;
                    if (husband != null && husband != gedPerson.XRefID)
                    {
                        spouseId = husband;
                    }
                }

                if (!string.IsNullOrEmpty(spouseId))
                {
                    familyDetails.Add(new Family
                    {
                        Type = FamilyType.Spouse,
                        Id = spouseId,
                        Label = NameFromGed(GedcomDb.Individuals.Find(i => i.XRefID == spouseId)?.GetName())
                    });
                }

                foreach (var child in familyRecord.Children)
                {
                    familyDetails.Add(new Family
                    {
                        Type = FamilyType.Child,
                        Id = child,
                        Label = NameFromGed(GedcomDb.Individuals.Find(i => i.XRefID == child)?.GetName())
                    });
                }
            }

            var eventList = new List<PersonEvent>();

            foreach (var gedEvent in gedPerson.Events.OrderBy(e => e.Date))
            {
                eventList.Add(new PersonEvent
                {
                    EventDate = gedEvent?.Date?.DateString,
                    Place = gedEvent?.Place?.Name,
                    Detail = gedEvent?.EventType switch
                    {
                        GedcomEventType.Birth => "Birth",
                        GedcomEventType.DEAT => "Death",
                        _ => gedEvent?.Classification ?? gedEvent?.EventName
                    }
                });
            }

            var personDetails = new PersonDetails
            {
                Title = NameFromGed(gedPerson.GetName()),
                Family = familyDetails,
                Events = eventList
            };

            return personDetails;
        }
    }
}
