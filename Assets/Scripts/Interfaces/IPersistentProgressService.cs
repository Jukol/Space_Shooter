using Data;

namespace Interfaces
{
    public interface IPersistentProgressService : IService
    {
        Progress Progress { get; set; }
    }
}
