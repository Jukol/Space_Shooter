using Data;
using EnemyScripts;
using Infrastructure.Signals;
using Interfaces;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace HUD
{
    public class HudData : MonoBehaviour, ISavedProgressWriter
    {
        [SerializeField] private TMP_Text level;
        [SerializeField] private TMP_Text wave;
        [SerializeField] private PlayerHealthBar playerHealthBar;
        [SerializeField] private KillCount killCount;

        private SpawnManager _spawnManager;
        private SignalBus _signalBus;

        public void Init(
            SignalBus signalBus, 
            string sceneName, 
            Player player, 
            int waveNumber, 
            int myKillCount)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<WaveCompleted>(ChangeWaveNumber);
            level.text = sceneName;
            wave.text = "Wave " + waveNumber;
            playerHealthBar.Init(player);
            killCount.Init(myKillCount);
        }

        public void LoadProgress(Progress progress)
        {
            killCount.killCounter = progress.lastState.killCount;
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lastState.killCount = killCount.killCounter;
        }

        private void OnDisable()
        {
            if (_spawnManager != null)
            {
                _signalBus.Unsubscribe<WaveCompleted>(ChangeWaveNumber);
            }
        }

        private void ChangeWaveNumber(WaveCompleted args)
        {
            int waveNumber = args.WaveNumber;
            waveNumber++;
            wave.text = "Wave " + waveNumber;
        }
    }
}
