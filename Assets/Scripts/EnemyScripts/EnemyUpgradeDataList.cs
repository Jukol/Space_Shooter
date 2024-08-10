using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScripts
{
    [CreateAssetMenu(fileName = "EnemyUpgradeDataList", menuName = "ScriptableObjects/EnemyUpgradeDataList", order = 2)]
    public class EnemyUpgradeDataList : ScriptableObject
    {
        [ShowInInspector]
        public EnemyShipUpgrade[] shipUpgrades;
    }

    [Serializable]
    public class EnemyShipUpgrade
    {
        public EnemyUpgradeData[] enemyUpgrades;
    }
}