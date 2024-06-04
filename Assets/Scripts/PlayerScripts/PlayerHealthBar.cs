using UnityEngine;

namespace PlayerScripts
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject healthUnit;
        [SerializeField] private float distanceBetweenUnits;
        
        private Player _player;

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
            GameObject[] healthUnits = GameObject.FindGameObjectsWithTag("HealthUnit");
            for (int i = 0; i < healthUnits.Length; i++)
            {
                Destroy(healthUnits[i].gameObject);
            }

            float initialXPosition = 0;

            for (int i = 0; i < _player.Health; i++)
            {
                GameObject thisHealthUnit = Instantiate(healthUnit, transform, false);
                thisHealthUnit.transform.localPosition = new Vector2(initialXPosition, 0);
                initialXPosition += distanceBetweenUnits;
            }
        }
    }
}
