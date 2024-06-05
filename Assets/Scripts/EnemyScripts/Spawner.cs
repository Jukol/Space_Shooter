using System;
using System.Collections;
using Background;
using Data;
using DG.Tweening;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace EnemyScripts
{
    public class Spawner : MonoBehaviour
    {
        public static Action OnAllShipsKilled;
        public EnemyPlaceHolder[] enemyPlaceHolders;
        public int ID => id;
        public static Action OnAllInPlace { get; set; }

        [SerializeField] private int id;
        [SerializeField] private Transform gridStartPosition;
        [SerializeField] private GameObject positionsParent;
        [SerializeField] private GameObject shipPrefab;
        [SerializeField] private float seconds;
        [SerializeField] private float timeToGetToPosition;
        [SerializeField] private GameObject maneuvering;

        private IBackgroundAdjuster _adjuster;
        private Vector3 _initialPosition;

        private Vector3 _initialScale;
        private WaitForSeconds _intervalBetweenShips;

        private int _killedShips;

        private IGameFactory _gameFactory;
        private Progress _progress;
        private ISaveLoadService _saveLoadService; 

        public void Init(IGameFactory gameFactory, Progress progress, ISaveLoadService saveLoadService)
        {
            _intervalBetweenShips = new WaitForSeconds(seconds);
            _adjuster = AllServices.Container.Single<IBackgroundAdjuster>();
            _progress = progress;
            _saveLoadService = saveLoadService;

            ResizeWindow();
            InitializePosition();
            ArrangeAndInitEnemyPlaceHolders();
            StartCoroutine(GetShipsInPlace(gameFactory));

            Enemy.OnDestroy += KilledShipsCounter;
        }

        private void InitializePosition()
        {
            Transform transform1 = transform;
            Vector3 position = transform1.position;
            _initialPosition = position;
            position = new Vector3(0, position.y / _adjuster.ResizeFactor, 0);
            transform1.position = position;
        }

        private void ResizeWindow()
        {
            Vector3 localScale = positionsParent.transform.localScale;
            _initialScale = localScale;
            localScale *= _adjuster.ResizeFactor;
            positionsParent.transform.localScale = localScale;
        }

        private void ArrangeAndInitEnemyPlaceHolders()
        {
            for (int i = 0; i < enemyPlaceHolders.Length; i++)
            {
                enemyPlaceHolders[i].Position = enemyPlaceHolders[i].transform.position;
                enemyPlaceHolders[i].enemyStatus = _progress.lastState.wrapperOfListOfSpawners.WrapperOfStatuses[id].ListOfStatuses[i];
            }
        }

        private void OnDisable()
        {
            Enemy.OnDestroy -= KilledShipsCounter;
            positionsParent.transform.localScale = _initialScale;
            transform.position = _initialPosition;
        }

        private IEnumerator GetShipsInPlace(IGameFactory gameFactory)
        {
            for (int i = 0; i < enemyPlaceHolders.Length; i++)
            {
                if (enemyPlaceHolders[i].enemyStatus.dead)
                {
                    yield return null;

                    if (i == enemyPlaceHolders.Length - 1)
                    {
                        yield return new WaitForSeconds(timeToGetToPosition);
                        AllInPosition();
                        yield break;
                    }
                    else
                    {
                        continue;
                    }
                }

                Enemy enemy = gameFactory.CreateEnemy(gridStartPosition, enemyPlaceHolders[i]);

                if (i != enemyPlaceHolders.Length - 1)
                {
                    enemy.transform.DOMove(enemyPlaceHolders[i].Position, timeToGetToPosition);
                    yield return _intervalBetweenShips;
                }
                else if (i == enemyPlaceHolders.Length - 1)
                {
                    Tween getToPosition = enemy.transform.DOMove(enemyPlaceHolders[i].Position, timeToGetToPosition);
                    getToPosition.OnComplete(AllInPosition);
                }
            }
        }

        private void AllInPosition()
        {
            OnAllInPlace?.Invoke();
        }

        private void KilledShipsCounter()
        {
            _killedShips++;
            if (_killedShips % enemyPlaceHolders.Length == 0)
            {
                OnAllShipsKilled?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
