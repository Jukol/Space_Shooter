using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject healthUnit;
        [SerializeField] private float distanceBetweenUnits;
        
        private Player _player;
        private List<GameObject> _healthUnits = new();

        public void Init(Player player)
        {
            _player = player;
            Player.OnHealthUpdate += DrawHealthUnits;
            DrawHealthUnits();
        }

        private void OnDisable()
        {
            Player.OnHealthUpdate -= DrawHealthUnits;
        }

        private void DrawHealthUnits()
        {
            if (_healthUnits.Count == 0) 
                CreateHealthBar();

            DeactivateCells();
            
            for (int i = 0; i < _player.Health; i++) 
                _healthUnits[i].SetActive(true);
        }

        private void DeactivateCells()
        {
            for (int i = 0; i < _healthUnits.Count; i++) 
                _healthUnits[i].gameObject.SetActive(false);
        }

        private void CreateHealthBar()
        {
            float initialXPosition = 0;
            
            for (int i = 0; i < _player.Health; i++)
            {
                GameObject thisHealthUnit = Instantiate(healthUnit, transform, false);
                thisHealthUnit.transform.localPosition = new Vector2(initialXPosition, 0);
                initialXPosition += distanceBetweenUnits;
                _healthUnits.Add(thisHealthUnit);
            }
        }
    }
}
