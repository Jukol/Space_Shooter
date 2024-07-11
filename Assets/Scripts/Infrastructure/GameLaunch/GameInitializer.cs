using Infrastructure.Wrappers;
using Interfaces;
using Logic;
using Zenject;

namespace Infrastructure.GameLaunch
{
    public class GameInitializer
    {
        [Inject] public LoadingCurtain Curtain;
        [Inject] public string InitialLevel;
        [Inject (Id = "PlayerHealth")] public int InitialPlayerHealth;
        [Inject] public SpawnersWrapper SpawnersWrapper;
        [Inject (Id = "InitialWave")] public int InitialWave;
        [Inject] public IPersistentProgressService ProgressService;
        [Inject] public ISaveLoadService SaveLoadService;
        [Inject] public IGameFactory GameFactory;
    }
}