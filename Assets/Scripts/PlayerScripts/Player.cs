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
        [SerializeField] private Transform[] socketPlaceholders;

        private PlayerUpgradeDataList _playerUpgradeDataList;

        private CameraShake _cameraShake;
        private bool _explosionStarted;
        private IMovable _movable;
        private IShootable[] _shootables;
        [Inject] private ISaveLoadService _saveLoadService;
        private CurrentScreen _currentScreen;
        
        private int _playerUpgradeLevel;

        private float fireRate;
        private int bulletDamage;
        private float bulletSpeed;
        private Transform[] sockets;
        private ParticleSystem myParticleSystem;

        public void Init(CameraShake cameraShake, CurrentScreen currentScreen, PlayerUpgradeDataList playerUpgradeDataList, int playerUpgradeLevel)
        {
            _cameraShake = cameraShake;
            _currentScreen = currentScreen;
            _playerUpgradeDataList = playerUpgradeDataList;
            _playerUpgradeLevel = playerUpgradeLevel;
            
            Width = transform.GetComponent<SpriteRenderer>().bounds.size.x;
            Height = transform.GetComponent<SpriteRenderer>().bounds.size.y;

            Animator = GetComponent<Animator>();
            _shootables = GetComponents<IShootable>();

            GetDataFromUpgrade();

            GetComponent<IMovable>().Init(currentScreen);

            _explosionStarted = false;

            GetToStartPosition();
        }

        public void LoadProgress(Progress progress)
        {
            Health = progress.lastState.playerHealth;
            OnHealthUpdate?.Invoke();
            _playerUpgradeLevel = progress.lastState.playerUpgradeLevel;
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lastState.playerHealth = Health;
            progress.lastState.playerUpgradeLevel = _playerUpgradeLevel;
        }

        private void GetDataFromUpgrade()
        {
            fireRate = _playerUpgradeDataList.playerUpgrades[_playerUpgradeLevel].fireRate;
            bulletDamage = _playerUpgradeDataList.playerUpgrades[_playerUpgradeLevel].bulletDamage;
            bulletSpeed = _playerUpgradeDataList.playerUpgrades[_playerUpgradeLevel].bulletSpeed;
            sockets = _playerUpgradeDataList.playerUpgrades[_playerUpgradeLevel].sockets;
            spriteRenderer.sprite = _playerUpgradeDataList.playerUpgrades[_playerUpgradeLevel].playerSprite;
            Animator.runtimeAnimatorController = _playerUpgradeDataList.playerUpgrades[_playerUpgradeLevel].playerAnimatorController;
            myParticleSystem = _playerUpgradeDataList.playerUpgrades[_playerUpgradeLevel].particleSystem;

            for (int i = 0; i < socketPlaceholders.Length; i++)
            {
                socketPlaceholders[i].localPosition = sockets[i].position;
                IShootable shootable = _shootables[i];
                shootable.Init(fireRate, bulletDamage, bulletSpeed, socketPlaceholders[i], myParticleSystem);
            }
        }

        public void StartShooting()
        {
            foreach (IShootable shootable in _shootables)
            {
                shootable.Shoot();
            }
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
            }
        }

        public void Upgrade()
        {
            int maxUpgrades = _playerUpgradeDataList.playerUpgrades.Length - 1;
            if (_playerUpgradeLevel < maxUpgrades)
            {
                _playerUpgradeLevel++;
            }
            else
            {
                Debug.Log("No more upgrades available!");
                return;
            }

            GetDataFromUpgrade();

            Debug.Log($"Upgraded to level {_playerUpgradeLevel}");

            _saveLoadService.SaveProgress();
        }
    }
}
