using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Background
{
    public class BackgroundMover : MonoBehaviour, IJumpUppable
    {
        [SerializeField] protected float speed = 0.5f;
        
        private const float GapCrutch = 0.1f;
        private BackgroundCompositor _compositor;
        private float _myHeight;
        private float _offset;
        private SpriteRenderer _spriteRenderer;
        private IBackgroundAdjuster _adjuster;

        public void Update()
        {
            Move();
            if (!(transform.position.y <= -(_myHeight + _offset)))
                return;
            
            JumpUp();
        }
        
        public void Init(IBackgroundAdjuster adjuster)
        {
            _adjuster = adjuster;
            _myHeight = _adjuster.Height;
            _offset = _adjuster.VerticalOffset;
        }

        private void JumpUp()
        {
            float moveUpY = _myHeight * 2 - _offset - GapCrutch;
            transform.position = new Vector2(0, moveUpY);
        }

        private void Move()
        {
            Transform myTransform = transform;
            myTransform.position += -myTransform.up * (Time.deltaTime * speed);
        }
    }
}
