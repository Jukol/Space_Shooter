using UnityEngine;
namespace Background
{
    public class BackgroundAdjuster : IBackgroundAdjuster
    {
        private readonly Camera _camera;
        private readonly SpriteRenderer _spriteRenderer;

        public BackgroundAdjuster(Camera camera, SpriteRenderer spriteRenderer)
        {
            _camera = camera;
            _spriteRenderer = spriteRenderer;
            ScreenAdjustmentData();
        }

        public float BackgroundsHeight { get; private set; }
        public float VerticalOffset { get; private set; }
        public float ResizeFactor { get; private set; }

        private void ScreenAdjustmentData()
        {
            float screenHeight = _camera.orthographicSize * 2;
            float screenWidth = screenHeight / Screen.height * Screen.width;

            Sprite sprite = _spriteRenderer.sprite;

            ResizeFactor = screenWidth / sprite.bounds.size.x;
            
            BackgroundsHeight = _spriteRenderer.bounds.size.y * ResizeFactor;
            VerticalOffset = (screenHeight - BackgroundsHeight) * 0.5f;
        }
    }
}
