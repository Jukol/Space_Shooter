using EnemyScripts;
using Infrastructure.GameLaunch;
using Infrastructure.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        
        private GameInitializer _gameInitializer;

        private int _completedSpawnManagerId;
        private GameStateMachine _gameStateMachine;
        private SpawnManager _spawnManager;

        public void Init(GameInitializer gameInitializer, GameStateMachine gameStateMachine, SpawnManager spawnManager)
        {
            _gameInitializer = gameInitializer;
            _gameStateMachine = gameStateMachine;
            _spawnManager = spawnManager;
        }

        private void OnEnable()
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            gameObject.SetActive(false);
        }


        public void SelectLevel(int level)
        {
            Destroy(_spawnManager.gameObject);
            
            _gameInitializer.ProgressService.Progress.lastState.levelToLoad = "Level " + level;
            _gameInitializer.ProgressService.Progress.lastState.waveToLoad = 0;
            
            _gameInitializer.ProgressService.Progress.lastState.spawnManagerIndex = level + 2;
            _gameInitializer.SaveLoadService.SaveProgress();
            
            string nextSceneName = "Level " + level;

            if (SceneManager.sceneCountInBuildSettings >= level + 1)
            {
                _gameStateMachine.Enter<LoadLevelState, string>(nextSceneName);
            }
            else
            {
                Debug.Log("Game Over!");
            }
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
    }
}
