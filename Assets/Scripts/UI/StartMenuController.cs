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

        public void Init(SpawnManager spawnManager, Player player, GameInitializer gameInitializer, GameStateMachine gameStateMachine)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() => OnStartButtonClicked (spawnManager, player));
            selectLevelButton.onClick.AddListener(OnSelectLevelButtonClicked);
            
            levelSelector.Init(gameInitializer, gameStateMachine, spawnManager);
            levelSelector.gameObject.SetActive(false);
        }

        private void OnSelectLevelButtonClicked()
        {
            levelSelector.gameObject.SetActive(true);
        }

        private void OnStartButtonClicked(SpawnManager spawnManager, Player player)
        {
            gameObject.SetActive(false);
            spawnManager.Launch();
            player.gameObject.SetActive(true);
        }
    }
}
