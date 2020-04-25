using System.Collections.Generic;
using ReactNet.Repository;

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
        public FamilyType Type { get; set; }
        public string Id { get; set; }
        public string Label { get; set; }
    }

    public class PersonNote
    {
        public string Detail { get; set; }
    }

    public class PersonDetails
    {
        public string Title { get; set; }
        public List<Family> Family { get; set; }
        public List<PersonEvent> Events { get; set; }
    }
}
