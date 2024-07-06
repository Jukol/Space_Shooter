using Interfaces;
using UnityEngine;

namespace Background
{
    public class BackgroundResizer : MonoBehaviour, IResizable
    {
        public void Resize(IBackgroundAdjuster backgroundAdjuster)
        {
            float resizeFactor = backgroundAdjuster.ResizeFactor;

            Transform mTransform = transform;

            Vector3 localScale = mTransform.localScale;
            float xScale = localScale.x;
            float yScale = localScale.y;

            localScale = new Vector3(xScale * resizeFactor, yScale * resizeFactor, 1);
            mTransform.localScale = localScale;
        }
    }
}
