using System;
using UnityEngine;

namespace PlayerScripts
{
    [Serializable]
    public class PlayerUpgradeData : MonoBehaviour
    {
        public Sprite playerSprites;
        public RuntimeAnimatorController playerAnimatorControllers;
        public Sprite bulletSprites;
        public float bulletSpeeds;
        public float bulletDamages;
        public float fireRates;
    }
}