using EnemyScripts;
using Infrastructure;
using Infrastructure.GameLaunch;
using Infrastructure.States;
using PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StartMenuController : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button selectLevelButton;
        [SerializeField] private LevelSelector levelSelector;
        
        private SpawnManager _spawnManager;
        private GameInitializer _gameInitializer;
        private GameStateMachine _gameStateMachine;

        public void Init(SpawnManager spawnManager, Player player, GameInitializer gameInitializer, GameStateMachine gameStateMachine)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() => OnStartButtonClicked (spawnManager, player));
            selectLevelButton.onClick.AddListener(OnSelectLevelButtonClicked);
            
            _spawnManager = spawnManager;
            _gameInitializer = gameInitializer;
            _gameStateMachine = gameStateMachine;
            
            levelSelector.gameObject.SetActive(false);
        }

        private void OnSelectLevelButtonClicked()
        {
            levelSelector.gameObject.SetActive(true);
            levelSelector.Init(_gameInitializer, _gameStateMachine, _spawnManager);
        }

        private void OnStartButtonClicked(SpawnManager spawnManager, Player player)
        {
            gameObject.SetActive(false);
            spawnManager.Launch(player);
            player.gameObject.SetActive(true);
            player.StartShooting();
        }
    }
}
