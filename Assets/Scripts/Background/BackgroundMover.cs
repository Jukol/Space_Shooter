using Infrastructure.Services;
using UnityEngine;
namespace Background
{
    public class BackgroundMover : MonoBehaviour, IMoveUppable
    {
        [SerializeField] protected float backgroundSpeed = 0.5f;
        private readonly float _gapCrutch = 0.1f;

        private IBackgroundAdjuster _adjuster;

        private BackgroundCompositor _backgroundCompositor;
        private float _myHeight;
        private float _offset;
        private SpriteRenderer _spriteRenderer;

        protected void Update()
        {
            Move();
            if (!(transform.position.y <= -(_myHeight + _offset)))
            {
                return;
            }
            MoveUp();
        }

        public void Init()
        {
            _adjuster = AllServices.Container.Single<IBackgroundAdjuster>();

            _myHeight = _adjuster.Height;
            _offset = _adjuster.VerticalOffset;
        }

        public void MoveUp()
        {
            float moveUpY = _myHeight * 2 - _offset - _gapCrutch;
            transform.position = new Vector2(0, moveUpY);
        }

        public void Move()
        {
            Transform myTransform = transform;
            myTransform.position += -myTransform.up * (Time.deltaTime * backgroundSpeed);
        }
    }
}
