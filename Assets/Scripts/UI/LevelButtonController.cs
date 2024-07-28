using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelButtonController : MonoBehaviour
    {
        [SerializeField] private GameObject levelText;
        [SerializeField] private GameObject lockIcon;

        private void Awake()
        {
            levelText.SetActive(false);
            lockIcon.SetActive(true);
        }

        public void SetLevelText(int level)
        {
            levelText.SetActive(true);
            levelText.GetComponent<TMP_Text>().text = level.ToString();
            lockIcon.SetActive(false);
        }
    }
}
