using EnemyScripts;
using Infrastructure.Services;
using Infrastructure.Services.SaveLoad;
using TMPro;
using UnityEngine;
using Zenject;

namespace HUD
{
    public class KillCount : MonoBehaviour
    {
        public int killCounter;
        [SerializeField] private TextMeshProUGUI killCounterField;
        [Inject] private ISaveLoadService _saveLoadService;
        

        public void Init(int count)
        {
            killCounterField.text = count.ToString();
            Enemy.OnDestroy += Counter;
        }

        private void OnDisable()
        {
            Enemy.OnDestroy -= Counter;
            
        }

        private void Counter()
        {
            killCounter++;
            killCounterField.text = killCounter.ToString();
            _saveLoadService.SaveProgress();
        }
    }
}
