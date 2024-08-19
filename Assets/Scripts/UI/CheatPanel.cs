using EnemyScripts;
using Infrastructure.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class CheatPanel : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button levelButtonPrefab;
        [SerializeField] private Transform levelButtonHolder;

        private SpawnManager _spawnManager;
        private GameStateMachine _gameStateMachine;

        public void Init(SpawnManager spawnManager, GameStateMachine gameStateMachine)
        {
            _spawnManager = spawnManager;
            _gameStateMachine = gameStateMachine;
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
            GenerateLevelButtons();
        }

        private void GenerateLevelButtons()
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings - 1; i++)
            {
                Button levelButton = Instantiate(levelButtonPrefab, levelButtonHolder);
                int level = i + 1;
                levelButton.GetComponent<LevelButtonController>().SetLevelText(level);
                levelButton.onClick.AddListener(() => LoadLevel(level));
            }
        }

        private void LoadLevel(int level)
        {
            Destroy(_spawnManager.gameObject);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            
            string levelToLoad = "Level " + level;
            
            _gameStateMachine.Enter<LoadLevelState, string>(levelToLoad);
        }
    }
}
