using UnityEngine;

namespace Infrastructure
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip playerHit;
        [SerializeField] private AudioClip smallExplosion;
        [SerializeField] private AudioClip playerExplosion;
        public static SoundManager Instance
        {
            get;
            private set;
        }

        private void Awake()
        {
            Instance = this;
        }

        public void HitSound()
        {
            audioSource.clip = playerHit;
            audioSource.Play();
        }

        public void SmallExplosion()
        {
            audioSource.clip = smallExplosion;
            audioSource.Play();
        }

        public void PlayerExplosion()
        {
            audioSource.clip = playerExplosion;
            audioSource.Play();
        }
    }
}
