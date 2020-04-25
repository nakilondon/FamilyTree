using System.Collections.Concurrent;

namespace ReactNet.Repository
{
    public interface IGedcom
    {
        ConcurrentDictionary<string, PersonDb> CreatePersonDbFromGedcom(string gedcomFile);
    }
}
