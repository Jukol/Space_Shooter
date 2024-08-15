using System;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EnemyScripts
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Inject]
        public void Construct(ISaveLoadService saveLoadService) => 
            _saveLoadService = saveLoadService;

        public static event Action OnDestroy;
        
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();

        [SerializeField] private float startHealth;
        [SerializeField] private EnemyScriptableObject shipData;
        [SerializeField] private RectTransform healthBar;
        [SerializeField] private GameObject shipExplosion, wounded, whiteSmoke;
        [SerializeField] private Transform[] socketPlaceholders;
        private bool useDoubleShooter;
        [SerializeField] private AudioSource audioSource;

        private int _damagedValue;
        private bool _dead;

        private int _currentHealth;

        private Slider _healthSlider;

        private bool _woundedAnim, _smokeAnim;
        private int _woundedValue;
        private ISaveLoadService _saveLoadService;
        private EnemyUpgradeData _enemyUpgradeData;
        private IShootable[] _shootables;

        private Spawner _spawner;
        
        private float _fireRate;
        private int _bulletDamage;
        private float _bulletSpeed;
        private Transform[] _sockets;
        private ParticleSystem _myParticleSystem;
        private Sprite _bulletSprite;
        private Sprite _enemySprite;

        private void OnEnable()
        {
            _woundedAnim = false;
        }

        public void Init(int health, EnemyUpgradeData enemyUpgradeData)
        {
            _enemyUpgradeData = enemyUpgradeData;

            _currentHealth = health;
            
            _woundedValue = (int)(0.5f * _currentHealth);
            _damagedValue = (int)(0.1f * _currentHealth);

            SetSlider();

            wounded.SetActive(false);
            whiteSmoke.SetActive(false);

            GetDataFromEnemyUpgrade();
            
            UpdateStatus();
            
            DamageEffects();

            StartShooting();
        }

        private void StartShooting()
        {
            foreach (IShootable shootable in _shootables)
            {
                shootable.Shoot();
            }
        }

        private void GetDataFromEnemyUpgrade()
        {
            _fireRate = _enemyUpgradeData.fireRate;
            _bulletDamage = _enemyUpgradeData.bulletDamage;
            _bulletSpeed = _enemyUpgradeData.bulletSpeed;
            _sockets = _enemyUpgradeData.sockets;
            _myParticleSystem = _enemyUpgradeData.myParticleSystem;
            _bulletSprite = _enemyUpgradeData.bulletSprite;
            _enemySprite = _enemyUpgradeData.playerSprite;
            SpriteRenderer.sprite = _enemySprite;
            audioSource.clip = _enemyUpgradeData.shootSound;
            useDoubleShooter = _enemyUpgradeData.useDoubleShooter;
            
            if (!useDoubleShooter)
            {
                EnemyDoubleShooter doubleShooter = GetComponent<EnemyDoubleShooter>();
                DestroyImmediate(doubleShooter);
            }
            
            _shootables = GetComponents<IShootable>();
            
            for (int i = 0; i < _sockets.Length; i++)
            {
                socketPlaceholders[i].localPosition = _sockets[i].position;
            }
            
            if (!useDoubleShooter)
            {
                for (int i = 0; i < _sockets.Length; i++)
                {
                    _shootables[i].Init(_fireRate, _bulletDamage, _bulletSpeed, socketPlaceholders[0], _myParticleSystem, _bulletSprite);
                }
                return;
            }
            
            _shootables[0].Init(_fireRate, _bulletDamage, _bulletSpeed, socketPlaceholders[0], _myParticleSystem, _bulletSprite, socketPlaceholders[1]);
        }

        private void SetSlider()
        {
            _healthSlider = healthBar.GetComponent<Slider>();
            _healthSlider.value = _currentHealth / startHealth;
        }

        public void Damage(int damageAmount)
        {
            if (_currentHealth >= 1)
                _currentHealth -= damageAmount;
            else if (_currentHealth < 0) 
                _currentHealth = 0;

            UpdateStatus();

            _saveLoadService.SaveProgress();

            _healthSlider.value = _currentHealth / startHealth;

            DamageEffects();

            if (_currentHealth != 0 || _dead)
                return;
            
            InitiateDestruction();
        }

        private void InitiateDestruction()
        {
            GameObject explosion = Instantiate(shipExplosion);
            explosion.transform.position = transform.position;
            Destroy(explosion, 1f);

            UpdateStatus();

            OnDestroy?.Invoke();

            _dead = true;

            Destroy(gameObject);
        }

        private void UpdateStatus()
        {
            transform.parent.GetComponent<EnemyPlaceHolder>()
                .UpdateStatus(_currentHealth, _saveLoadService);
        }

        private void DamageEffects()
        {
            if (_currentHealth < _woundedValue && !_woundedAnim)
            {
                wounded.gameObject.SetActive(true);
                _woundedAnim = true;
            }

            if (_currentHealth < _damagedValue && !_smokeAnim)
            {
                whiteSmoke.gameObject.SetActive(true);
                _smokeAnim = true;
            }
        }
    }
}
