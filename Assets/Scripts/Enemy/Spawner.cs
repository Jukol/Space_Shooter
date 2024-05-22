using System;
using System.Collections;
using Background;
using DG.Tweening;
using Infrastructure.Services;
using UnityEngine;
namespace Enemy
{
    public class Spawner : MonoBehaviour
    {
        public static Action OnAllShipsKilled;

        [SerializeField] private int id;
        [SerializeField] private Transform gridStartPosition;
        [SerializeField] private GameObject positionsParent;
        [SerializeField] private GameObject shipPrefab;
        [SerializeField] private float seconds;
        [SerializeField] private ShipPlaceHolder[] positionGrid;
        [SerializeField] private float timeToGetToPosition;
        [SerializeField] private GameObject maneuvering;

        private IBackgroundAdjuster _adjuster;
        private Vector3 _initialPosition;

        private Vector3 _initialScale;
        private WaitForSeconds _intervalBetweenShips;

        private int _killedShips;
        public int ID => id;
        public static Action OnAllInPlace { get; set; }

        private void Start()
        {
            _intervalBetweenShips = new WaitForSeconds(seconds);
        }

        private void OnEnable()
        {
            _adjuster = AllServices.Container.Single<IBackgroundAdjuster>();

            Vector3 localScale = positionsParent.transform.localScale;
            _initialScale = localScale;
            localScale *= _adjuster.ResizeFactor;
            positionsParent.transform.localScale = localScale;

            Transform transform1 = transform;
            Vector3 position = transform1.position;
            _initialPosition = position;
            position = new Vector3(0, position.y / _adjuster.ResizeFactor, 0);
            transform1.position = position;

            ArrangeShipPlaceHolders();

            StartCoroutine(GetShipsInPlace(shipPrefab));

            EnemyShipBehavior.OnDestroy += KilledShipsCounter;
        }

        private void ArrangeShipPlaceHolders()
        {
            foreach (var shipPlaceHolder in positionGrid)
            {
                shipPlaceHolder.Position = shipPlaceHolder.transform.position;
                shipPlaceHolder.ShipDead = false;
            }
        }

        private void OnDisable()
        {
            EnemyShipBehavior.OnDestroy -= KilledShipsCounter;
            positionsParent.transform.localScale = _initialScale;
            transform.position = _initialPosition;
        }

        private IEnumerator GetShipsInPlace(GameObject shipPfb)
        {
            for (int i = 0; i < positionGrid.Length; i++)
            {
                EnemyShipBehavior ship = Instantiate(shipPfb, gridStartPosition.position, Quaternion.Euler(0, 0, 180)).GetComponent<EnemyShipBehavior>();
                ship.transform.SetParent(positionGrid[i].transform, true);
                positionGrid[i].Ship = ship;

                if (i != positionGrid.Length - 1)
                {
                    if (positionGrid[i].ShipDead) 
                        yield break;
                    ship.transform.DOMove(positionGrid[i].Position, timeToGetToPosition);
                    yield return _intervalBetweenShips;
                }
                else if (i == positionGrid.Length - 1)
                {
                    if (positionGrid[i].ShipDead) 
                        yield break;
                    Tween getToPosition = ship.transform.DOMove(positionGrid[i].Position, timeToGetToPosition);
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
            if (_killedShips % positionGrid.Length == 0)
            {
                OnAllShipsKilled?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
