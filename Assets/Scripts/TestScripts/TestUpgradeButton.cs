using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

public class TestUpgradeButton : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            UpgradePlayer();
        });
    }

    private void UpgradePlayer()
    {
        Player player = FindObjectOfType<Player>();
        player.Upgrade();
    }
}
