using EnemyScripts;
using Infrastructure.GameLaunch;
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
        private GameInitializer _gameInitializer;

        public void Init(SpawnManager spawnManager, GameStateMachine gameStateMachine, GameInitializer gameInitializer)
        {
            _spawnManager = spawnManager;
            _gameStateMachine = gameStateMachine;
            _gameInitializer = gameInitializer;
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
            _gameInitializer.ProgressService.Progress.lastState.levelToLoad = levelToLoad;
            _gameInitializer.SaveLoadService.SaveProgress();
            _gameStateMachine.Enter<LoadLevelState, string>(levelToLoad);
        }
    }
}
