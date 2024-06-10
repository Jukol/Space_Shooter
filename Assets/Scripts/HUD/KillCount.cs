using EnemyScripts;
using Infrastructure.Services;
using Infrastructure.Services.SaveLoad;
using TMPro;
using UnityEngine;
namespace HUD
{
    public class KillCount : MonoBehaviour
    {
        public int killCounter;
        [SerializeField] private TextMeshProUGUI killCounterField;
        private ISaveLoadService _saveLoadService;
        
        private void Start()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

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
