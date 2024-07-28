using System.Collections.Generic;

namespace PlayerScripts
{
    public class UpgradePlayer
    {
        private int currentUpgradeLevel = 0;
        private Dictionary<int, PlayerUpgradeData> upgradeData;
        private readonly PlayerUpgradeData playerUpgradeData;
        
        public UpgradePlayer(PlayerUpgradeData playerUpgradeData)
        {
            this.playerUpgradeData = playerUpgradeData;
            upgradeData = new Dictionary<int, PlayerUpgradeData>();
            upgradeData.Add(0, playerUpgradeData);
        }

        public void Upgrade()
        {
            currentUpgradeLevel++;
            UpgradeUpgradables(currentUpgradeLevel);
        }

        private void UpgradeUpgradables(int currentUpgradeLevel)
        {
            
        }
    }
}