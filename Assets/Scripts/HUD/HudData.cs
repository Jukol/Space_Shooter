using EnemyScripts;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace HUD
{
    public class HudData : MonoBehaviour
    {
        [SerializeField] private TMP_Text level;
        [SerializeField] private TMP_Text wave;
        [SerializeField] private PlayerHealthBar playerHealthBar;

        private SpawnManager _spawnManager;

        private string test;

        // private void OnEnable()
        // {
        //     _spawnManager = FindObjectOfType<SpawnManager>();
        //     _spawnManager.WaveChanged += ChangeWaveNumber;
        //     level.text = SceneManager.GetActiveScene().name;
        // }

        private void OnDisable()
        {
            if (_spawnManager != null)
            {
                _spawnManager.WaveChanged -= ChangeWaveNumber;
            }
        }

        public void Init(SpawnManager spawnManager, string sceneName, Player player, int waveNumber)
        {
            spawnManager.WaveChanged += ChangeWaveNumber;
            level.text = sceneName;
            ChangeWaveNumber(waveNumber);
            playerHealthBar.Init(player);
        }

        private void ChangeWaveNumber(int waveNumber)
        {
            waveNumber++;
            wave.text = "Wave " + waveNumber;
        }
    }
}
