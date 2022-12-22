using System;
using System.CodeDom;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Items
{
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour
    {
        public GameObject projectile;
        public AudioClip shootSound;
        public event Action<Collider> OnHit; 

        [SerializeField] protected ProjectileProperties projectileProperties;

        public List<Collider> ignoreColliders = new ();
        
        private AudioSource audioSource;

        public Vector3 shootPosition => transform.position + transform.rotation * projectileProperties.ShootOffset;

        protected virtual void Awake()
        {
            ignoreColliders.AddRange(GetComponentsInChildren<Collider>());
        }
        
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public Projectile Shoot()
        {
            var projectileInstance = Instantiate(projectile, shootPosition, transform.rotation).GetComponent<Projectile>();
            projectileInstance.properties = projectileProperties;
            projectileInstance.OnHit += hit => OnHit?.Invoke(hit);
            audioSource.PlayOneShot(shootSound);
            
            // Ignore collisions with the weapon
            var projectileCollider = projectileInstance.GetComponent<Collider>();
            foreach (var weaponCollider in ignoreColliders)
                Physics.IgnoreCollision(projectileCollider, weaponCollider);
            
            return projectileInstance;
        }
    }
}