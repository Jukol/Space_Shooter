using Infrastructure.Services;
namespace Background
{
    public interface IBackgroundAdjuster : IService
    {
        public float Height { get; }
        public float VerticalOffset { get; }

        public float ResizeFactor { get; }
    }
}
