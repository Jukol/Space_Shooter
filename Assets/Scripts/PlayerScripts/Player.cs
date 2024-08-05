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
        [SerializeField] private Transform[] socketPlaceholders;
        [SerializeField] private AudioSource audioSource;

        private PlayerUpgradeDataList _playerUpgradeDataList;

        private CameraShake _cameraShake;
        private bool _explosionStarted;
        private IMovable _movable;
        private IShootable[] _shootables;
        [Inject] private ISaveLoadService _saveLoadService;
        private CurrentScreen _currentScreen;

        private int _ship;
        private int _playerUpgradeLevel;

        private float fireRate;
        private int bulletDamage;
        private float bulletSpeed;
        private Transform[] sockets;
        private ParticleSystem myParticleSystem;
        private Sprite sprite;
        private GameObject explosion;

        public void Init(CameraShake cameraShake, CurrentScreen currentScreen, PlayerUpgradeDataList playerUpgradeDataList, int playerShip, int playerUpgradeLevel)
        {
            _cameraShake = cameraShake;
            _currentScreen = currentScreen;
            _playerUpgradeDataList = playerUpgradeDataList;
            _ship = playerShip;
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
            _ship = progress.lastState.playerShip;
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lastState.playerHealth = Health;
            progress.lastState.playerUpgradeLevel = _playerUpgradeLevel;
            progress.lastState.playerShip = _ship;
        }

        private void GetDataFromUpgrade()
        {
            fireRate = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].fireRate;
            bulletDamage = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].bulletDamage;
            bulletSpeed = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].bulletSpeed;

            sockets = null;
            sockets = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].sockets;
            
            spriteRenderer.sprite = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].playerSprite;
            Animator.runtimeAnimatorController = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].playerAnimatorController;
            myParticleSystem = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].myParticleSystem;
            sprite = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].bulletSprite;
            explosion = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].explosion;
            audioSource.clip = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].shootSound;

            for (int i = 0; i < sockets.Length; i++)
            {
                socketPlaceholders[i].localPosition = sockets[i].position;
                IShootable shootable = _shootables[i];
                shootable.Init(fireRate, bulletDamage, bulletSpeed, socketPlaceholders[i], myParticleSystem, sprite);
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
                        Instantiate(explosion, transform.position, Quaternion.identity);
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
            foreach (IShootable shootable in _shootables)
            {
                shootable.StopShooting();
            }
            
            int maxUpgrades = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades.Length - 1;
            if (_playerUpgradeLevel < maxUpgrades)
            {
                _playerUpgradeLevel++;
            }
            else
            {
                Debug.Log("No more upgrades available!");
                StartShooting();
                return;
            }

            GetDataFromUpgrade();
            StartShooting();

            Debug.Log($"Upgraded to level {_playerUpgradeLevel}");

            _saveLoadService.SaveProgress();
        }
        
        public void ChangeShip()
        {
            _ship++;
            _playerUpgradeLevel = 0;
            GetDataFromUpgrade();
            
            _saveLoadService.SaveProgress();
        }
    }
}
