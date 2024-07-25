using UnityEngine;
using UnityEngine.UIElements;

namespace UI.LevelSelector
{
    public class LevelSelectorController : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private StyleSheet levelButtonStyle;
        [SerializeField] private int levelCount = 10;

        private VisualElement levelIconHolder;
        private Button closeButton;

        private void OnEnable()
        {
            VisualElement root = _uiDocument.rootVisualElement;
            levelIconHolder = root.Q<VisualElement>("LevelIconHolder");
            closeButton = root.Q<Button>("CloseButton");
            closeButton.clicked += OnCloseButtonClicked;
            
            CreateLevelIcons();
        }

        private void OnCloseButtonClicked()
        {
            closeButton.clicked -= OnCloseButtonClicked;
            Destroy(gameObject);
        }

        private void CreateLevelIcons()
        {
            for (int i = 0; i < levelCount; i++)
            {
                Button levelButton = new Button();
                levelButton.text = $"Level {(i + 1)}";
                levelButton.AddToClassList("LevelButton");
                levelIconHolder.Add(levelButton);
            }
        }
    }
}
