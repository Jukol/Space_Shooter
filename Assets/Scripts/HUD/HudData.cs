using Data;
using EnemyScripts;
using Interfaces;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace HUD
{
    public class HudData : MonoBehaviour, ISavedProgressWriter
    {
        [SerializeField] private TMP_Text level;
        [SerializeField] private TMP_Text wave;
        [SerializeField] private PlayerHealthBar playerHealthBar;
        [SerializeField] private KillCount killCount;

        private SpawnManager _spawnManager;

        public void Init(
            SpawnManager spawnManager, 
            string sceneName, 
            Player player, 
            int waveNumber, 
            int myKillCount)
        {
            spawnManager.WaveChanged += ChangeWaveNumber;
            level.text = sceneName;
            ChangeWaveNumber(waveNumber);
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
                _spawnManager.WaveChanged -= ChangeWaveNumber;
            }
        }

        private void ChangeWaveNumber(int waveNumber)
        {
            waveNumber++;
            wave.text = "Wave " + waveNumber;
        }
    }
}
