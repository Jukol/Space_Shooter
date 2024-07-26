using Infrastructure;
using Infrastructure.GameLaunch;
using Infrastructure.States;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        
        private GameInitializer _gameInitializer;

        private int _completedSpawnManagerId;
        private GameStateMachine _gameStateMachine;

        public void Init(GameInitializer gameInitializer, GameStateMachine gameStateMachine)
        {
            _gameInitializer = gameInitializer;
            _gameStateMachine = gameStateMachine;
        }

        private void OnEnable()
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            gameObject.SetActive(false);
        }


        public void SelectLevel(Button button)
        {
            _gameInitializer.ProgressService.Progress.lastState.levelToLoad = "Level " + (_completedSpawnManagerId + 2);
            int wave = _gameInitializer.ProgressService.Progress.lastState.waveToLoad = 0;
            
            _gameInitializer.ProgressService.Progress.lastState.spawnManagerIndex = _completedSpawnManagerId + 2;
            _gameInitializer.SaveLoadService.SaveProgress();
            
            _completedSpawnManagerId += 2;
            string nextSceneName = "Level " + _completedSpawnManagerId;

            if (SceneManager.sceneCountInBuildSettings >= _completedSpawnManagerId + 1)
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
