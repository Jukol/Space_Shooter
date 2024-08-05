using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerScripts
{
    [CreateAssetMenu(fileName = "PlayerUpgradeDataList", menuName = "ScriptableObjects/PlayerUpgradeDataList", order = 1)]
    public class PlayerUpgradeDataList : ScriptableObject
    {
        [ShowInInspector]
        public ShipUpgrade[] shipUpgrades;
    }

    [Serializable]
    public class ShipUpgrade
    {
        public PlayerUpgradeData[] playerUpgrades;
    }
}