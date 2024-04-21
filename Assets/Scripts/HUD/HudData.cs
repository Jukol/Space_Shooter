using Enemy;
using Player;
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

        private void OnEnable()
        {
            _spawnManager = FindObjectOfType<SpawnManager>();
            _spawnManager.WaveChanged += ChangeWaveNumber;
            level.text = SceneManager.GetActiveScene().name;
        }

        private void OnDisable()
        {
            _spawnManager.WaveChanged -= ChangeWaveNumber;
        }

        public void Init(SpawnManager spawnManager, string sceneName, Player.Player player)
        {
            spawnManager.WaveChanged += ChangeWaveNumber;
            level.text = sceneName;
            playerHealthBar.Init(player);
        }

        private void ChangeWaveNumber(int waveNumber)
        {
            wave.text = "Wave " + waveNumber;
        }
    }
}
