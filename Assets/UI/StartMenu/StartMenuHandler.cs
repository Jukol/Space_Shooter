using EnemyScripts;
using Infrastructure.GameLaunch;
using PlayerScripts;
using UI.LevelSelector;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuHandler : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private LevelSelectorController levelSelectorController;

    private Button startButton;
    private Button selectLevelButton;
    private VisualElement topBox;
    
    private SpawnManager _spawnManager;
    private Player _player;

    public void Init(SpawnManager spawnManager, Player player)
    {
        VisualElement root = uiDocument.rootVisualElement;
        
        startButton = root.Q<Button>("StartButton");
        selectLevelButton = root.Q<Button>("LevelSelectorButton");
        topBox = root.Q<VisualElement>("TopBox");
        
        _spawnManager = spawnManager;
        _player = player;
        
        startButton.clicked += OnStartButtonClicked;
        selectLevelButton.clicked += OnSelectLevelButtonClicked;
    }

    private void OnSelectLevelButtonClicked()
    {
        Instantiate(levelSelectorController);
    }

    private void OnStartButtonClicked()
    {
        topBox.style.display = DisplayStyle.None;
        
        _spawnManager.Launch();
        _player.gameObject.SetActive(true);
    }
}
