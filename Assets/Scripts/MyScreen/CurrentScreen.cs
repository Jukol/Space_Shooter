using Interfaces;
using UnityEngine;
namespace MyScreen
{
    public class CurrentScreen : IGetSizeable, IService
    {
        public CurrentScreen(Camera myCamera)
        {
            Camera camera = myCamera;
            Vector3 screenBounds = camera.ScreenToWorldPoint(
                new Vector3(
                    Screen.width, 
                    Screen.height, 
                    camera.transform.position.z)
                );
            Width = screenBounds.x;
            Height = screenBounds.y;
        }

        public float Width { get; }
        public float Height { get; }

        public ScreenBounds GetBoundsForObject(IGetSizeable go)
        {
            ScreenBounds bounds = new ScreenBounds
            {
                Left = -Width + go.Width / 2,
                Right = Width - go.Width / 2,
                Top = Height - go.Height / 2,
                Bottom = -Height + go.Height / 2
            };

            return bounds;
        }
    }
}
