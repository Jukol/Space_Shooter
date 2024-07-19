using System;
using System.Collections;
using System.Linq;
using Data;
using DG.Tweening;
using Interfaces;
using UnityEngine;

namespace EnemyScripts
{
    public class Spawner : MonoBehaviour
    {
        public static Action OnAllShipsKilled;
        public EnemyPlaceHolder[] enemyPlaceHolders;
        public static Action OnAllInPlace { get; set; }

        public int id;
        
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

        private int _spawnManagerId;

        public void Init(
            int spawnManagerId,
            IGameFactory gameFactory, 
            Progress progress,
            IBackgroundAdjuster adjuster)
        {
            _intervalBetweenShips = new WaitForSeconds(seconds);
            _adjuster = adjuster;
            _progress = progress;
            _spawnManagerId = spawnManagerId;

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
                    enemyPlaceHolders[i].enemyStatus = 
                    _progress.lastState
                        .spawnWrapperHolder
                        .chiefWrapper.SpawnersWrappers[_spawnManagerId].WrapperOfStatuses[id].ListOfStatuses[i];
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
                    continue;
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
            if (enemyPlaceHolders.All(placeholder => placeholder.enemyStatus.dead))
            {
                OnAllShipsKilled?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
