using System.Collections.Generic;
using GameJam.Player;
using UnityEngine;

namespace GameJam.Items
{
    [RequireComponent(typeof(Weapon), typeof(AudioSource))]
    public class EnergyGun : GrabItem, IUsablePowered
    {
        [SerializeField] private float energyCost = 5f;
        [SerializeField] private AudioClip shootSound;
     
        private Weapon _weapon;
        private AudioSource _audioSource;
        
        private void Start()
        {
            _weapon = GetComponent<Weapon>();
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void Use(Battery battery)
        {
            if(!battery.TryDischarge(energyCost))
                return;

            var shotProjectile = _weapon.Shoot();
            _audioSource.PlayOneShot(shootSound);
            
            shotProjectile.OnHit += OnHitCallback;
        }
        
        private void OnHitCallback(Collider other)
        {
            Debug.Log("Shot " + other.name);
        }
    }
}

