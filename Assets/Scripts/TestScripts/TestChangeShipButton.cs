using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace TestScripts
{
    public class TestChangeShipButton : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                ChangeShip();
            });
        }

        private void ChangeShip()
        {
            Player player = FindObjectOfType<Player>();
            player.ChangeShip();
        }
    }
}
