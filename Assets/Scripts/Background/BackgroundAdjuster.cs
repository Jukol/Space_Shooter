using Interfaces;
using UnityEngine;
using Zenject;

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

        public float Height { get; private set; }
        public float VerticalOffset { get; private set; }
        public float ResizeFactor { get; private set; }

        private void ScreenAdjustmentData()
        {
            float screenHeight = _camera.orthographicSize * 2;
            float screenWidth = screenHeight / Screen.height * Screen.width;

            Sprite sprite = _spriteRenderer.sprite;

            ResizeFactor = screenWidth / sprite.bounds.size.x;
            
            Height = _spriteRenderer.bounds.size.y * ResizeFactor;
            VerticalOffset = (screenHeight - Height) * 0.5f;
        }
    }
}
