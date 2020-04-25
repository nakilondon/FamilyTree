using System;
using System.Collections.Generic;

namespace ReactNet.Repository
{
    public enum Gender
    {
        Male = 0,
        Female = 1
    }

    public enum EventType
    {
        Birth,
        Death,
        Other
    }

    public class EventDate
    {
        public bool Certain { get; set; }
        public DateTime Date { get; set; }
    }
    public class PersonEvent
    {
        public EventType Type { get; set; }
        public string EventDate { get; set; }
        public string Place { get; set; }
        public List<string> Source { get; set; }
        public string Detail { get; set; }
    }

    public class PersonDb
    {
        public string Id { get; set; }
        public List<string> Spouses { get; set; }
        public List<string> Parents { get; set; }
        public List<string> Children { get; set; }
        public List<string> Siblings { get; set; }
        public string PreferredName { get; set; }
        public string[] GivenNames { get; set; }
        public List<PersonEvent> Events { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DeathDate { get; set; }
    }
}
