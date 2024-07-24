using EnemyScripts;
using Infrastructure.GameLaunch;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuHandler : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private Button startButton;
    private VisualElement topBox;
    
    private SpawnManager _spawnManager;
    private Player _player;

    public void Init(SpawnManager spawnManager, Player player)
    {
        VisualElement root = uiDocument.rootVisualElement;
        
        startButton = root.Q<Button>("StartButton");
        topBox = root.Q<VisualElement>("TopBox");
        
        _spawnManager = spawnManager;
        _player = player;
        
        startButton.clicked += OnStartButtonClicked;
    }

    private void OnStartButtonClicked()
    {
        topBox.style.display = DisplayStyle.None;
        
        _spawnManager.Launch();
        _player.gameObject.SetActive(true);
    }
}
