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
        [SerializeField] private bool useDoubleShooter;

        private int _damagedValue;
        private bool _dead;

        private int _currentHealth;

        private Slider _healthSlider;

        private bool _woundedAnim, _smokeAnim;
        private int _woundedValue;
        private ISaveLoadService _saveLoadService;
        private EnemyUpgradeDataList _enemyUpgradeDataList;
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

        public void Init(int health, EnemyUpgradeDataList enemyUpgradeDataList)
        {
            _enemyUpgradeDataList = enemyUpgradeDataList;

            _currentHealth = health;
            
            _woundedValue = (int)(0.5f * _currentHealth);
            _damagedValue = (int)(0.1f * _currentHealth);

            SetSlider();

            wounded.SetActive(false);
            whiteSmoke.SetActive(false);

            if (!useDoubleShooter)
            {
                EnemyDoubleShooter doubleShooter = GetComponent<EnemyDoubleShooter>();
                DestroyImmediate(doubleShooter);
            }
            
            _shootables = GetComponents<IShootable>();
            
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
            _fireRate = _enemyUpgradeDataList.shipUpgrades[0].enemyUpgrades[0].fireRate;
            _bulletDamage = _enemyUpgradeDataList.shipUpgrades[0].enemyUpgrades[0].bulletDamage;
            _bulletSpeed = _enemyUpgradeDataList.shipUpgrades[0].enemyUpgrades[0].bulletSpeed;
            _sockets = _enemyUpgradeDataList.shipUpgrades[0].enemyUpgrades[0].sockets;
            _myParticleSystem = _enemyUpgradeDataList.shipUpgrades[0].enemyUpgrades[0].myParticleSystem;
            _bulletSprite = _enemyUpgradeDataList.shipUpgrades[0].enemyUpgrades[0].bulletSprite;
            _enemySprite = _enemyUpgradeDataList.shipUpgrades[0].enemyUpgrades[0].playerSprite;
            SpriteRenderer.sprite = _enemySprite;
            
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
