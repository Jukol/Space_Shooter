using System;
using Data;
using Infrastructure;
using Interfaces;
using MyScreen;
using UnityEngine;
using Zenject;

namespace PlayerScripts
{
    public class Player : MonoBehaviour, IDamageable, IGetSizeable, IAnimatable, ISavedProgressWriter
    {
        public static Action OnHealthUpdate;
        public int Health { get; private set; }
        public Animator Animator { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject megaExplosion;

        private CameraShake _cameraShake;
        private bool _explosionStarted;
        private IMovable _movable;
        private IShootable[] _shootables;
        [Inject] private ISaveLoadService _saveLoadService;
        private CurrentScreen _currentScreen;
        
        private int currentUpgradeLevel = 0;
        [Inject] private PlayerUpgradeData _playerUpgradeData;

        public void Init(CameraShake cameraShake, CurrentScreen currentScreen)
        {
            Width = transform.GetComponent<SpriteRenderer>().bounds.size.x;
            Height = transform.GetComponent<SpriteRenderer>().bounds.size.y;
            Animator = GetComponent<Animator>();

            _currentScreen = currentScreen;

            GetComponent<IMovable>().Init(currentScreen);

            _shootables = GetComponents<IShootable>();

            foreach (IShootable shootable in _shootables)
            {
                shootable.Init();
            }

            _explosionStarted = false;
            _cameraShake = cameraShake;
            
            GetToStartPosition();
        }
        
        public void GetToStartPosition()
        {
            ScreenBounds bounds = _currentScreen.GetBoundsForObject(this);
            transform.position = new Vector3(0, bounds.Bottom, 0);
        }

        public void Damage(int amount)
        {
            Health -= amount;
            StartCoroutine(_cameraShake.Shake(0.2f, 0.05f));
            _saveLoadService.SaveProgress();
            OnHealthUpdate?.Invoke();
        }

        public void LoadProgress(Progress progress)
        {
            Health = progress.lastState.playerHealth;
            OnHealthUpdate?.Invoke();
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lastState.playerHealth = Health;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Damage(1);
                if (Health < 1)
                {
                    StartCoroutine(_cameraShake.Shake(1, 0.1f));
                    if (!_explosionStarted)
                    {
                        _explosionStarted = true;
                        Instantiate(megaExplosion, transform.position, Quaternion.identity);
                        SoundManager.Instance.PlayerExplosion();
                        spriteRenderer.enabled = false;
                        Destroy(gameObject, 2f);
                    }
                }
            }

            if (collision.CompareTag("Upgrade"))
            {
                Upgrade();
                Debug.Log("Upgrade collided with player");
            }
        }
        
        public void Upgrade()
        {
            // currentUpgradeLevel++;
            // spriteRenderer.sprite = _playerUpgradeData.playerSprites[currentUpgradeLevel];
            // Animator.runtimeAnimatorController = _playerUpgradeData.playerAnimatorControllers[currentUpgradeLevel];
        }
    }
}
