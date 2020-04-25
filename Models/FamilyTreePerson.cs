using System;
using System.Collections.Generic;

namespace ReactNet.Models
{
    public enum AdviserPlacementType
    {
        Auto = 0,
        Left = 2,
        Right = 3
    }

    public class FamilyTreePerson
    {
        public string Id { get; set; }
        public List<string> Spouses { get; set; }
        public List<string> Parents { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string RelativeItem { get; set; }
        public short Position { get; set; }
        public string ItemTitleColor { get; set; }
        public AdviserPlacementType PlacementType { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
