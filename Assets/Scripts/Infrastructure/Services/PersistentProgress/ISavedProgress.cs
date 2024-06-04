using Data;
namespace Infrastructure.Services.PersistentProgress
{
    public interface ISavedProgressReader
    {
        void LoadProgress(Progress progress);
    }
    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress(Progress progress);
    }
}
