using System;
using System.Collections;
using Background;
using Data;
using Infrastructure.Factory;
using Infrastructure.Signals;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace EnemyScripts
{
    public class SpawnManager : MonoBehaviour, ISavedProgressWriter
    {
        public event Action<int> WaveChanged;
        
        public Spawner[] spawners;

        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;
        private IPersistentProgressService _progress;
        private IBackgroundAdjuster _adjuster;
        private SignalBus _signalBus;

        private int Wave { get; set; }
        
        [Inject]
        public void Construct(
            ISaveLoadService saveLoadService, 
            IGameFactory gameFactory, 
            IPersistentProgressService progress, 
            IBackgroundAdjuster adjuster,
            SignalBus signalBus)
        {
            _saveLoadService = saveLoadService;
            _gameFactory = gameFactory;
            _progress = progress;
            _adjuster = adjuster;
            _signalBus = signalBus;
        }

        public void Start()
        {
            StartCoroutine(SpawnerEnumerator(_gameFactory, _progress.Progress, _adjuster));
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lastState.waveToLoad = Wave;
            progress.lastState.spawnersWrapper = _progress.Progress.lastState.spawnersWrapper;
        }

        public void LoadProgress(Progress progress)
        {
            var currentScene = SceneManager.GetActiveScene().name;
            
            if (currentScene == progress.lastState.levelToLoad)
            {
                Wave = progress.lastState.waveToLoad;
                _progress.Progress.lastState.spawnersWrapper = progress.lastState.spawnersWrapper;
            }
        }

        private IEnumerator SpawnerEnumerator(IGameFactory gameFactory, Progress progress, IBackgroundAdjuster adjuster)
        {
            int wave = progress.lastState.waveToLoad;
            
            while (true)
            {
                for (int i = wave; i < spawners.Length; i++)
                {
                    spawners[i].gameObject.SetActive(true);
                    Wave = i;
                    _signalBus.Fire(new WaveCompleted() {WaveNumber = Wave});
                    spawners[i].Init(gameFactory, progress, adjuster);
                    _saveLoadService.SaveProgress();
                    int i1 = i;
                    yield return new WaitUntil(() => spawners[i1].gameObject.activeSelf == false);
                }
                
                _signalBus.Fire<LevelCompleted>();
                Debug.Log("Level completed!");
            }
        }
    }
}
