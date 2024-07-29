using UnityEngine;

namespace PlayerScripts
{
    [CreateAssetMenu(fileName = "PlayerUpgradeDataList", menuName = "ScriptableObjects/PlayerUpgradeDataList", order = 1)]
    public class PlayerUpgradeDataList : ScriptableObject
    {
        public PlayerUpgradeData[] playerUpgrades;
    }
}