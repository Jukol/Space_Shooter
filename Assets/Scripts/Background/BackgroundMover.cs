using Infrastructure.Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace Background
{
    public class BackgroundMover : MonoBehaviour, IJumpUppable
    {
        [SerializeField] protected float speed = 0.5f;
        
        private const float GapCrutch = 0.1f;

        private IBackgroundAdjuster _adjuster;

        private BackgroundCompositor _compositor;
        private float _myHeight;
        private float _offset;
        private SpriteRenderer _spriteRenderer;

        protected void Update()
        {
            Move();
            if (!(transform.position.y <= -(_myHeight + _offset)))
                return;
            
            JumpUp();
        }

        public void Init()
        {
            _adjuster = AllServices.Container.Single<IBackgroundAdjuster>();

            _myHeight = _adjuster.Height;
            _offset = _adjuster.VerticalOffset;
        }

        public void JumpUp()
        {
            float moveUpY = _myHeight * 2 - _offset - GapCrutch;
            transform.position = new Vector2(0, moveUpY);
        }

        public void Move()
        {
            Transform myTransform = transform;
            myTransform.position += -myTransform.up * (Time.deltaTime * speed);
        }
    }
}
