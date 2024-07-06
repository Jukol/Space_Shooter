using Data;

namespace Interfaces
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress();

        Progress LoadProgress();
    }
}
