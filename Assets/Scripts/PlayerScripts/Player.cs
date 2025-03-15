using System;
using System.Collections;
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
        [SerializeField] private bool undamageable;
        [SerializeField] private float cooldownTime = 3;
        [SerializeField] private float blinkInterval = 0.1f;

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
        private Sprite bulletSprite;
        private GameObject explosion;
        private Coroutine undamageableCoroutine;
        private Color originalColor;
        private Progress _progress;

        public void Init(CameraShake cameraShake, CurrentScreen currentScreen, PlayerUpgradeDataList playerUpgradeDataList, int playerShip, int playerUpgradeLevel)
        {
            _cameraShake = cameraShake;
            _currentScreen = currentScreen;
            _playerUpgradeDataList = playerUpgradeDataList;
            _ship = playerShip;
            _playerUpgradeLevel = playerUpgradeLevel;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].playerSprite;
            
            Width = spriteRenderer.bounds.size.x;
            Height = spriteRenderer.bounds.size.y;

            Animator = GetComponent<Animator>();
            _shootables = GetComponents<IShootable>();

            GetDataFromUpgrade();

            GetComponent<IMovable>().Init(currentScreen, this);

            _explosionStarted = false;
            originalColor = spriteRenderer.color;

            GetToStartPosition();
        }

        public void LoadProgress(Progress progress)
        {
            _progress = progress;
            Health = progress.lastState.playerHealth;
            OnHealthUpdate?.Invoke();
            _playerUpgradeLevel = progress.lastState.playerUpgradeLevel;
            _ship = progress.lastState.playerShip;
        }

        public void UpgradeHealth()
        {
            _progress.lastState.playerHealth++;
            Health = _progress.lastState.playerHealth;
            _saveLoadService.SaveProgress();
            OnHealthUpdate?.Invoke();
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
            bulletSprite = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].bulletSprite;
            explosion = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].explosion;
            audioSource.clip = _playerUpgradeDataList.shipUpgrades[_ship].playerUpgrades[_playerUpgradeLevel].shootSound;

            for (int i = 0; i < sockets.Length; i++)
            {
                socketPlaceholders[i].localPosition = sockets[i].position;
                IShootable singleShootable = _shootables[i];
                singleShootable.Init(fireRate, bulletDamage, bulletSpeed, socketPlaceholders[i], myParticleSystem, bulletSprite);
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
            if (!undamageable)
            {
                Health -= amount;
                StartCoroutine(_cameraShake.Shake(0.2f, 0.05f));
                _saveLoadService.SaveProgress();
                OnHealthUpdate?.Invoke();
                Cooldown();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Damage(1);
            }
            
            if (Health <= 0)
            {
                HandlePlayerDeath();
            }
        }

        private void HandlePlayerDeath()
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
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        private void Cooldown()
        {
            if (undamageableCoroutine != null)
            {
                StopCoroutine(undamageableCoroutine);
            }
            undamageableCoroutine = StartCoroutine(CooldownCoroutine());
        }

        private IEnumerator CooldownCoroutine()
        {
            undamageable = true;
            float elapsedTime = 0f;

            while (elapsedTime < cooldownTime)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(blinkInterval);
                spriteRenderer.enabled = false;
                yield return new WaitForSeconds(blinkInterval);

                elapsedTime += blinkInterval * 2;
            }

            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
            undamageable = false;
            undamageableCoroutine = null;
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
