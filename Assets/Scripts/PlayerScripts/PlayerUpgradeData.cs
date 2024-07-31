using System;
using UnityEngine;

namespace PlayerScripts
{
    [Serializable]
    public class PlayerUpgradeData : MonoBehaviour
    {
        public float fireRate;
        public int bulletDamage;
        public float bulletSpeed;
        public Sprite playerSprite;
        public RuntimeAnimatorController playerAnimatorController;
        public Transform[] sockets;
        public Sprite bulletSprite;
    }
}