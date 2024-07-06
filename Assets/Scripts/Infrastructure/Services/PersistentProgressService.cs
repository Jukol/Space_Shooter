using Data;
using Interfaces;

namespace Infrastructure.Services
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public Progress Progress { get; set; }
    }
}
