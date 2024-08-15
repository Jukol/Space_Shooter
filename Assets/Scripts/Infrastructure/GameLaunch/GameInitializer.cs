using EnemyScripts;
using Interfaces;
using Logic;
using PlayerScripts;
using UI;
using Zenject;

namespace Infrastructure.GameLaunch
{
    public class GameInitializer
    {
        [Inject] public LoadingCurtain Curtain;
        [Inject] public string InitialLevel;
        [Inject (Id = "PlayerHealth")] public int InitialPlayerHealth;
        [Inject (Id = "SpawnManagerIndex")] public int SpawnManagerIndex;
        [Inject (Id = "InitialWave")] public int InitialWave;
        [Inject (Id = "InitialPlayerUpgradeLevel")] public int InitialPlayerUpgradeLevel;
        [Inject] public IPersistentProgressService ProgressService;
        [Inject] public ISaveLoadService SaveLoadService;
        [Inject] public IGameFactory GameFactory;
        [Inject] public SignalBus SignalBus;
        [Inject] public StartMenuController StartMenuController;
        [Inject] public PlayerUpgradeDataList PlayerUpgradeDataList;
        [Inject (Id = "PlayerShip")] public int PlayerShip;
    }
}