using Infrastructure.GameLaunch;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuHandler : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private GameBootstrapper gameBootstrapper;

    private Button startButton;
    private VisualElement topBox;

    private void Start()
    {
        VisualElement root = uiDocument.rootVisualElement;
        
        startButton = root.Q<Button>("StartButton");
        topBox = root.Q<VisualElement>("TopBox");
        
        startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
    }

    private void OnStartButtonClicked(ClickEvent evt)
    {
        topBox.style.display = DisplayStyle.None;
        
        gameBootstrapper.Launch();
    }
}
