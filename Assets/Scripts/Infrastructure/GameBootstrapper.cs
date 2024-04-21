using Ammo;
using Infrastructure.States;
using Logic;
using MyScreen;
using UnityEngine;
namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain curtain;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BulletContainer bulletParent;
        [SerializeField] private Camera shakingCamera;
        [SerializeField] private string initialLevel;
        [SerializeField] private int initialHealth;

        private CameraShake cameraShake;

        private Game _game;

        private void Awake()
        {
            LoadingCurtain loadingCurtain = Instantiate(curtain);
            SpriteRenderer sprRenderer = Instantiate(spriteRenderer);
            BulletContainer bulletContainer = Instantiate(bulletParent);
            Camera shkCamera = Instantiate(shakingCamera);

            cameraShake = shkCamera.GetComponent<CameraShake>();

            _game = new Game(this, 
                loadingCurtain,
                shkCamera, 
                sprRenderer, 
                bulletContainer, 
                cameraShake,
                initialLevel,
                initialHealth);
            
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}
