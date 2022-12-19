using System.Collections.Generic;
using GameJam.Player;
using UnityEngine;

namespace GameJam.Items
{
    public class EnergyGun : WeaponBase, IUsablePowered
    {
        [SerializeField] private float energyCost = 5f;
        
        public void Use(Battery battery)
        {
            if(!battery.TryDischarge(energyCost))
                return;
            
            var shotProjectile = Shoot();
            
            shotProjectile.OnHit += OnHitCallback;
        }
        
        private void OnHitCallback(Collider other)
        {
            Debug.Log("Shot " + other.name);
        }
    }
}

