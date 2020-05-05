using System.Collections.Generic;
using ReactNet.Repositories;

namespace ReactNet.Models
{
    public enum FamilyType
    {
        Spouse = 0,
        Parent = 1,
        Child = 2,
        Sibling = 3
    }
    public class Family
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }

    }

    public class PersonNote
    {
        public string Detail { get; set; }
    }

    public class PersonDetails
    {
        public string Id { get; set; }
        public string PreferredName { get; set; }
        public string FullName { get; set; }
        public string Birth { get; set; }
        public string Death { get; set; }
        public string Portrait { get; set; }
        public string Note { get; set; }
        public List<Family> Family { get; set; }
        public List<PersonEvent> Events { get; set; }
    }
}
