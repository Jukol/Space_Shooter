namespace Interfaces
{
    public interface IBackgroundAdjuster : IService
    {
        public float Height { get; }
        public float VerticalOffset { get; }
        public float ResizeFactor { get; }
    }
}
