using System;
using UnityEngine;

namespace EnemyScripts
{
    [Serializable]
    public class EnemyUpgradeData : MonoBehaviour
    {
        public float fireRate;
        public int bulletDamage;
        public float bulletSpeed;
        public Sprite playerSprite;
        //public RuntimeAnimatorController playerAnimatorController;
        public Transform[] sockets;
        public ParticleSystem myParticleSystem;
        public Sprite bulletSprite;
        //public GameObject explosion;
        //public AudioClip shootSound;
    }
}
