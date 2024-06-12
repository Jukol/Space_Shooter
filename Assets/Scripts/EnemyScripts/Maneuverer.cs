using DG.Tweening;
using UnityEngine;
namespace EnemyScripts
{
    public class Maneuverer : MonoBehaviour
    {
        [SerializeField] private DOTweenAnimation tweens;
        [SerializeField] private bool move;
        [SerializeField] private bool scale;
        [SerializeField] private bool rotate;

        private void Start()
        {
            tweens.GetTweens();
        }

        private void OnEnable()
        {
            Spawner.OnAllInPlace += StartTweens;
            Spawner.OnAllShipsKilled += RewindAndPause;
        }

        private void OnDisable()
        {
            Spawner.OnAllInPlace -= StartTweens;
            Spawner.OnAllShipsKilled -= RewindAndPause;
        }

        private void StartTweens()
        {
            if (move) tweens.DOPlayAllById("Move");

            if (scale) tweens.DOPlayAllById("Scale");

            if (rotate) tweens.DOPlayAllById("Rotate");
        }

        private void RewindAndPause()
        {
            DOTween.RewindAll();
            DOTween.PauseAll();
        }
    }
}
