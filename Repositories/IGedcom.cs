using System.Collections.Concurrent;

namespace ReactNet.Repositories
{
    public interface IGedcom
    {
        ConcurrentDictionary<string, PersonDb> CreatePersonDbFromGedcom(string gedcomFile);
    }
}
