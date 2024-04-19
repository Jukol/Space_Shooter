using System;
using System.Collections;
using Data;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Enemy
{
    public class SpawnManager : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private Spawner[] spawners;

        private ISaveLoadService _saveLoadService;

        public int Wave
        {
            get;
            set;
        }

        private void Start()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            StartCoroutine(SpawnerEnumerator());
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.lastLevelAndWave.waveToLoad = Wave;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (SceneManager.GetActiveScene().name == progress.lastLevelAndWave.levelToLoad)
            {
                Wave = progress.lastLevelAndWave.waveToLoad;
            }
        }

        public event Action<int> WaveChanged;

        private IEnumerator SpawnerEnumerator()
        {
            while (true)
            {
                for (int i = Wave; i < spawners.Length; i++)
                {
                    spawners[i].gameObject.SetActive(true);
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
