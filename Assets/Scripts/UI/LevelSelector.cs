using System.Collections.Generic;
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
        [SerializeField] private Button levelButtonPrefab;
        [SerializeField] private Transform levelButtonHolder;

        private GameInitializer _gameInitializer;

        private int _completedSpawnManagerId;
        private GameStateMachine _gameStateMachine;
        private SpawnManager _spawnManager;
        private bool _buttonsGenerated;
        private readonly List<Button> _levelButtons = new ();

        public void Init(GameInitializer gameInitializer, GameStateMachine gameStateMachine, SpawnManager spawnManager)
        {
            _gameInitializer = gameInitializer;
            _gameStateMachine = gameStateMachine;
            _spawnManager = spawnManager;
            _spawnManager.LevelCompleted += UpdateButtons;

            if (!_buttonsGenerated)
            {
                GenerateLevelButtons();
            }
        }

        private void UpdateButtons(SpawnManager spawnManager)
        {
            int levelToOpen = spawnManager.id;
            
            for (int i = 0; i < _levelButtons.Count; i++)
            {
                if (i <= levelToOpen)
                {
                    int level = i + 1;
                    UpdateButtonStatus(_levelButtons[i], level);
                }
            }
        }

        private void GenerateLevelButtons()
        {
            int currentLevel = _gameInitializer.ProgressService.Progress.lastState.spawnManagerIndex;
            
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings - 1; i++)
            {
                Button levelButton = Instantiate(levelButtonPrefab, levelButtonHolder);
                if (i <= currentLevel)
                {
                    int level = i + 1;
                    UpdateButtonStatus(levelButton, level);
                }
                
                _levelButtons.Add(levelButton);
            }

            _buttonsGenerated = true;
        }

        private void UpdateButtonStatus(Button levelButton, int level)
        {

            levelButton.GetComponent<LevelButtonController>().SetLevelText(level);
            levelButton.onClick.AddListener(() => SelectLevel(level));
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

        private void OnDestroy()
        {
            _spawnManager.LevelCompleted -= UpdateButtons;
        }
    }
}
