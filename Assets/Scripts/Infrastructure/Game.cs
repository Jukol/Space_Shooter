using Ammo;
using Infrastructure.Services;
using Infrastructure.States;
using Logic;
using MyScreen;
using UnityEngine;
namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(
            ICoroutineRunner coroutineRunner, 
            LoadingCurtain curtain, 
            Camera camera, 
            SpriteRenderer spriteRenderer, 
            BulletContainer bulletParent, 
            CameraShake cameraShake, 
            string initialLevel, 
            int initialHealth)
        {
            StateMachine = new GameStateMachine(
                new SceneLoader(coroutineRunner), 
                curtain, 
                AllServices.Container,
                camera, 
                spriteRenderer, 
                bulletParent, 
                cameraShake,
                initialLevel,
                initialHealth);
        }
    }
}
