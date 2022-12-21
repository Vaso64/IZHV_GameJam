using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Items
{
    public class Weapon : MonoBehaviour
    {
        public GameObject projectile;
        
        public event Action<Collider> OnHit; 

        [SerializeField] protected ProjectileProperties projectileProperties;

        public List<Collider> ignoreColliders = new ();

        public Vector3 shootPosition => transform.position + transform.rotation * projectileProperties.ShootOffset;

        protected virtual void Awake()
        {
            ignoreColliders.AddRange(GetComponentsInChildren<Collider>());
        }

        public Projectile Shoot()
        {
            var projectileInstance = Instantiate(projectile, shootPosition, transform.rotation).GetComponent<Projectile>();
            projectileInstance.properties = projectileProperties;
            projectileInstance.OnHit += hit => OnHit?.Invoke(hit);
            
            // Ignore collisions with the weapon
            var projectileCollider = projectileInstance.GetComponent<Collider>();
            foreach (var weaponCollider in ignoreColliders)
                Physics.IgnoreCollision(projectileCollider, weaponCollider);
            
            return projectileInstance;
        }
    }
}