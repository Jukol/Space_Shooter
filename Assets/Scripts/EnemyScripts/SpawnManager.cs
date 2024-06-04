using System;
using System.Collections;
using Data;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace EnemyScripts
{
    public class SpawnManager : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private Spawner[] spawners;

        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;
        private IPersistentProgressService _progress;

        public int Wave
        {
            get;
            set;
        }

        public void Init(IGameFactory gameFactory, IPersistentProgressService progress)
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            StartCoroutine(SpawnerEnumerator(gameFactory, progress.Progress));
            _progress = progress;
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lastState.waveToLoad = Wave;
            progress.lastState.listOfSpawnerEnemyStatusLists = _progress.Progress.lastState.listOfSpawnerEnemyStatusLists;
        }

        public void LoadProgress(Progress progress)
        {
            if (SceneManager.GetActiveScene().name == progress.lastState.levelToLoad)
            {
                Wave = progress.lastState.waveToLoad;
                _progress.Progress.lastState.listOfSpawnerEnemyStatusLists = progress.lastState.listOfSpawnerEnemyStatusLists;
            }
        }

        public event Action<int> WaveChanged;

        private IEnumerator SpawnerEnumerator(IGameFactory gameFactory, Progress progress)
        {
            while (true)
            {
                for (int i = Wave; i < spawners.Length; i++)
                {
                    spawners[i].gameObject.SetActive(true);
                    spawners[i].Init(gameFactory, progress, _saveLoadService);
                    WaveChanged?.Invoke(spawners[i].ID);
                    Wave = i;
                    _saveLoadService.SaveProgress();
                    int i1 = i;
                    yield return new WaitUntil(() => spawners[i1].gameObject.activeSelf == false);
                }
            }
        }
    }
}
