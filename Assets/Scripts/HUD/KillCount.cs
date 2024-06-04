using EnemyScripts;
using TMPro;
using UnityEngine;
namespace HUD
{
    public class KillCount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI killCounterField;
        private int _killCounter;


        private void OnEnable()
        {
            killCounterField = GetComponent<TextMeshProUGUI>();
            killCounterField.text = "0";
            Enemy.OnDestroy += Counter;
        }

        private void OnDisable()
        {
            Enemy.OnDestroy -= Counter;
        }

        private void Counter()
        {
            _killCounter++;
            killCounterField.text = _killCounter.ToString();
        }
    }
}
