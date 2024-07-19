using System;
using System.Collections;
using Data;
using Infrastructure.Signals;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace EnemyScripts
{
    public class SpawnManager : MonoBehaviour, ISavedProgressWriter
    {
        public int id;
        public Spawner[] spawners;
        public event Action<SpawnManager> LevelCompleted;

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

        public void Launch()
        {
            StartCoroutine(SpawnerEnumerator(_gameFactory, _progress.Progress, _adjuster));
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lastState.waveToLoad = Wave;

            progress.lastState.spawnManagerIndex = id;
        }

        public void LoadProgress(Progress progress)
        {
            var currentScene = SceneManager.GetActiveScene().name;
            
            if (currentScene == progress.lastState.levelToLoad)
            {
                Wave = progress.lastState.waveToLoad;
                
                _progress.Progress.lastState.spawnManagerIndex = progress.lastState.spawnManagerIndex;
            }
        }

        private IEnumerator SpawnerEnumerator(IGameFactory gameFactory, Progress progress, IBackgroundAdjuster adjuster)
        {
            int wave = progress.lastState.waveToLoad;

            for (int i = wave; i < spawners.Length; i++)
            {
                spawners[i].gameObject.SetActive(true);
                Wave = i;
                _signalBus.Fire(new WaveCompleted(i));
                spawners[i].Init(id,i, gameFactory, progress, adjuster);
                _saveLoadService.SaveProgress();
                int i1 = i;
                yield return new WaitUntil(() => spawners[i1].gameObject.activeSelf == false);
            }
                
            LevelCompleted?.Invoke(this);

            Destroy(gameObject);
        }
    }
}
