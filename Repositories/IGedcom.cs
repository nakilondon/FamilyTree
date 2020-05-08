using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ReactNet.Repositories
{
    public interface IGedcom
    {
        Task<ConcurrentDictionary<string, PersonDb>> CreatePersonDbFromGedcom();
    }
}
