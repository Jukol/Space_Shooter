using UnityEngine;

namespace Infrastructure
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
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

        public void PlayerExplosion()
        {
            audioSource.clip = playerExplosion;
            audioSource.Play();
        }
    }
}
