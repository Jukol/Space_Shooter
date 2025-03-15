using Interfaces;
using MyScreen;
using UnityEngine;
using Zenject;

namespace PlayerScripts
{
    public class Mover : MonoBehaviour, IMovable
    {
        private static readonly int AnimatorMove = Animator.StringToHash("Move");

        private IAnimatable _animator;
        private float _bottomBorder;

        private Camera _camera;
        private IGetSizeable _gameObjectSize;

        [Inject] private IInput _iInput;
        private float _lastYPosition;

        private float _leftBorder;
        private Vector3 _offsetDistance;
        private float _rightBorder;
        private IGetSizeable _screenBounds;
        private float _topBorder;

        private void Update()
        {
            _iInput.UserInput();
        }

        public void OnMouseDown()
        {
            //_offsetDistance = MousePositionInWorld() - transform.position;
        }

        private void OnMouseDrag()
        {
            Move();
        }

        public void Init(CurrentScreen currentScreen, IGetSizeable gameObjectSize)
        {
            _camera = Camera.main;

            _gameObjectSize = gameObjectSize;
            _animator = GetComponent<IAnimatable>();
            
            ScreenBounds borders = currentScreen.GetBoundsForObject(_gameObjectSize);

            _leftBorder = borders.Left;
            _rightBorder = borders.Right;
            _topBorder = borders.Top;
            _bottomBorder = borders.Bottom;
        }

        public void Move()
        {
            Vector3 positionWithinScreen = LimitByScreen();
            transform.position = positionWithinScreen;
            ForwardChecker();
        }

        private Vector2 LimitByScreen()
        {
            float limitByX = Mathf.Clamp(MousePositionInWorld().x - _offsetDistance.x, _leftBorder, _rightBorder);
            float limitByY = Mathf.Clamp(MousePositionInWorld().y - _offsetDistance.y, _bottomBorder, _topBorder);

            return new Vector3(limitByX, limitByY);
        }

        private void ForwardChecker()
        {
            float currentPosition = transform.position.y;

            if (currentPosition > _lastYPosition)
            {
                _animator.Animator.SetTrigger(AnimatorMove);
            }

            _lastYPosition = currentPosition;
        }

        private Vector3 MousePositionInWorld()
        {
            return _camera.ScreenToWorldPoint(new Vector3(
                _iInput.Horizontal,
                _iInput.Vertical,
                _camera.WorldToScreenPoint(transform.position).z)
            );
        }
    }
}
