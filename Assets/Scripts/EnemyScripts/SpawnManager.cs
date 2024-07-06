using System;
using System.Collections;
using Background;
using Data;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace EnemyScripts
{
    public class SpawnManager : MonoBehaviour, ISavedProgress
    {
        public Spawner[] spawners;

        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;
        private IPersistentProgressService _progress;
        private IBackgroundAdjuster _adjuster;

        public int Wave { get; set; }
        
        [Inject]
        public void Construct(ISaveLoadService saveLoadService, IGameFactory gameFactory, IPersistentProgressService progress, IBackgroundAdjuster adjuster)
        {
            _saveLoadService = saveLoadService;
            _gameFactory = gameFactory;
            _progress = progress;
            _adjuster = adjuster;
        }
        
        public void Launch()
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
            if (SceneManager.GetActiveScene().name == progress.lastState.levelToLoad)
            {
                Wave = progress.lastState.waveToLoad;
                _progress.Progress.lastState.spawnersWrapper = progress.lastState.spawnersWrapper;
            }
        }

        public event Action<int> WaveChanged;

        private IEnumerator SpawnerEnumerator(IGameFactory gameFactory, Progress progress, IBackgroundAdjuster adjuster)
        {
            while (true)
            {
                for (int i = Wave; i < spawners.Length; i++)
                {
                    spawners[i].gameObject.SetActive(true);
                    Wave = i;
                    WaveChanged?.Invoke(Wave);
                    spawners[i].Init(gameFactory, progress, adjuster);
                    _saveLoadService.SaveProgress();
                    int i1 = i;
                    yield return new WaitUntil(() => spawners[i1].gameObject.activeSelf == false);
                }
            }
        }
    }
}
