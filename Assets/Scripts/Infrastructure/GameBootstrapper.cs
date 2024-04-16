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
        
        private CameraShake cameraShake;

        private Game _game;

        private void Awake()
        {
            var loadingCurtain = Instantiate(curtain);
            var sprRenderer = Instantiate(spriteRenderer);
            var bulletContainer = Instantiate(bulletParent);
            var shkCamera = Instantiate(shakingCamera);

            cameraShake = shkCamera.GetComponent<CameraShake>();

            _game = new Game(this, 
                loadingCurtain,
                shkCamera, 
                sprRenderer, 
                bulletContainer, 
                cameraShake);
            
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}
