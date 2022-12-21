using System.Collections.Generic;
using GameJam.Player;
using UnityEngine;

namespace GameJam.Items
{
    [RequireComponent(typeof(Weapon))]
    public class EnergyGun : GrabItem, IUsablePowered
    {
        [SerializeField] private float energyCost = 5f;
     
        private Weapon _weapon;
        
        private void Start()
        {
            _weapon = GetComponent<Weapon>();
        }
        
        public void Use(Battery battery)
        {
            if(!battery.TryDischarge(energyCost))
                return;
            
            var shotProjectile = _weapon.Shoot();
            
            shotProjectile.OnHit += OnHitCallback;
        }
        
        private void OnHitCallback(Collider other)
        {
            Debug.Log("Shot " + other.name);
        }
    }
}

