using System.Collections.Generic;

namespace PlayerScripts
{
    public class UpgradePlayer
    {
        private int currentUpgradeLevel = 0;
        private readonly PlayerUpgradeData _playerUpgradeData;
        private readonly Player _player;
        public UpgradePlayer(PlayerUpgradeData playerUpgradeData, Player player)
        {
            _playerUpgradeData = playerUpgradeData;
            _player = player;
        }

        public void Upgrade()
        {
            currentUpgradeLevel++;
            UpgradeUpgradables(currentUpgradeLevel);
        }

        private void UpgradeUpgradables(int upgradeLevel)
        {
            _player.spriteRenderer.sprite = _playerUpgradeData.playerSprites[upgradeLevel];
            _player.Animator.runtimeAnimatorController = _playerUpgradeData.playerAnimatorControllers[upgradeLevel];
        }
    }
}