using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GeneGenie.Gedcom;
using GeneGenie.Gedcom.Enums;
using GeneGenie.Gedcom.Parser;

namespace ReactNet.Repository
{
    public class Gedcom : IGedcom
    {
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

        public ConcurrentDictionary<string, PersonDb> CreatePersonDbFromGedcom(string gedcomFile)
        {
            var personDictionary = new ConcurrentDictionary<string, PersonDb>();
            var gedcomRecordReader = GedcomRecordReader.CreateReader(gedcomFile);
            if (gedcomRecordReader.Parser.ErrorState != GedcomErrorState.NoError)
            {
                Console.WriteLine($"Could not read file, encountered error {gedcomRecordReader.Parser.ErrorState}.");
            }

            var gedcomDb = gedcomRecordReader.Database;

            foreach (var gedcomDbIndividual in gedcomDb.Individuals)
            {
                var gedName = gedcomDbIndividual.GetName();
                var personDb = new PersonDb
                {
                    Id = gedcomDbIndividual.XRefID,
                    Gender = gedcomDbIndividual.Sex == GedcomSex.Female ? Gender.Female : Gender.Male,
                    PreferredName = NameFromGed(gedName),
                    Surname = gedName.Surname,
                    GivenNames = gedName.Given.Split(' '),
                    Parents = new List<string>(),
                    Spouses = new List<string>(),
                    Children = new List<string>(),
                    Siblings = new List<string>(),
                    Events = new List<PersonEvent>()
                };

                if (gedcomDbIndividual.Birth != null || gedcomDbIndividual.Death != null)
                {
                    personDb.Description = "(";
                    if (gedcomDbIndividual.Birth?.Date?.DateTime1 != null)
                    {
                        personDb.BirthDate = (DateTime)gedcomDbIndividual.Birth.Date.DateTime1;   
                        personDb.Description += gedcomDbIndividual.Birth.Date.DateString;
                    }

                    
                    if (gedcomDbIndividual.Death?.Date?.DateTime1 != null)
                    {
                        personDb.DeathDate = (DateTime) gedcomDbIndividual.Death.Date.DateTime1;
                        personDb.Description += " - " + gedcomDbIndividual.Death.Date.DateString;
                    }
                    
                    personDb.Description += ")";
                }

                foreach (var family in gedcomDbIndividual.ChildIn)
                {
                    var parentsDb = gedcomDb
                        .Individuals.FindAll((i => i.SpouseIn != null && i.SpouseInFamily(family.Family)));

                    foreach (var parentDb in parentsDb)
                    {
                        if (!parentDb.Equals(gedcomDbIndividual))
                        {
                            personDb.Parents.Add(parentDb.XRefID);
                        }
                    }

                    var siblingsDb = gedcomDb
                        .Individuals.FindAll(i => i.ChildIn != null && i.ChildInFamily(family.Family));
                    foreach (var sibling in siblingsDb)
                    {
                        if (!sibling.Equals(gedcomDbIndividual))
                        {
                            personDb.Siblings.Add(sibling.XRefID);

                        }
                    }

                    foreach (var gedEvent in gedcomDbIndividual.Events)
                    {
                        var personEvent = new PersonEvent
                        {
                            Type = gedEvent.EventType switch
                            {
                                GedcomEventType.Birth => EventType.Birth,
                                GedcomEventType.DEAT => EventType.Death,
                                _ => EventType.Other,
                            },
                            EventDate = gedEvent.Date.DateString,
                            Source = new List<string>(),
                            Place = gedEvent.Place?.Name,
                            Detail = gedEvent.Classification
                        };

                        foreach (var source in gedEvent.Sources)
                        {
                            personEvent.Source.Add(source.Text);   
                        }

                        personDb.Events.Add(personEvent);
                    }
                }

                var familyRecords =
                    gedcomDb.Families.FindAll(f =>
                        f.Husband == gedcomDbIndividual.XRefID || f.Wife == gedcomDbIndividual.XRefID);
                foreach (var familyRecord in familyRecords)
                {
                    string spouseId = null;
                    var wife = familyRecord.Wife;
                    if (wife != null && wife != gedcomDbIndividual.XRefID)
                    {
                        spouseId = wife;
                    }
                    else
                    {
                        var husband = familyRecord.Husband;
                        if (husband != null && husband != gedcomDbIndividual.XRefID)
                        {
                            spouseId = husband;
                        }
                    }

                    if (!String.IsNullOrEmpty(spouseId))
                    {
                        personDb.Spouses.Add(spouseId);
                    }

                    foreach (var child in familyRecord.Children)
                    {
                        personDb.Children.Add(child);
                    }

                }

                personDictionary.TryAdd(personDb.Id, personDb);
            }

            return personDictionary;
        }
    }

}