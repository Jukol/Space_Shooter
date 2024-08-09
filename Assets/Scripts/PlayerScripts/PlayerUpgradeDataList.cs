using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerScripts
{
    [CreateAssetMenu(fileName = "PlayerUpgradeDataList", menuName = "ScriptableObjects/PlayerUpgradeDataList", order = 1)]
    public class PlayerUpgradeDataList : ScriptableObject
    {
        [ShowInInspector]
        public PlayerShipUpgrade[] shipUpgrades;
    }

    [Serializable]
    public class PlayerShipUpgrade
    {
        public PlayerUpgradeData[] playerUpgrades;
    }
}