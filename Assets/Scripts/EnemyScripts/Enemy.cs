using System;
using Infrastructure.Services;
using Infrastructure.Services.SaveLoad;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace EnemyScripts
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public static event Action OnDestroy;

        [SerializeField] private float startHealth;
        [SerializeField] private EnemyScriptableObject shipData;
        [SerializeField] private RectTransform healthBar;
        [SerializeField] private GameObject shipExplosion, wounded, whiteSmoke;
        
        private int _damagedValue;
        private bool _dead;

        private int _currentHealth;

        private Slider _healthSlider;

        private bool _woundedAnim, _smokeAnim;
        private int _woundedValue;
        private ISaveLoadService _saveLoadService;

        private Spawner _spawner;

        private void OnEnable() => 
            _woundedAnim = false;

        public void Init(int health)
        {
            _woundedValue = shipData.wounded;
            _damagedValue = shipData.damaged;

            _currentHealth = health;
            
            SetSlider();

            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();

            wounded.SetActive(false);
            whiteSmoke.SetActive(false);
            
            UpdateStatus();
            
            DamageEffects();
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

        private void UpdateStatus() => 
            transform.parent.GetComponent<EnemyPlaceHolder>().UpdateStatus(_currentHealth);

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
