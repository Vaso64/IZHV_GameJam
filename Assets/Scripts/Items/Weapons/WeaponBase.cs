using UnityEngine;

namespace GameJam.Items
{
    public class WeaponBase : ItemBase
    {
        public GameObject projectile;
        
        
        [SerializeField] protected ProjectileProperties projectileProperties;

        private Collider[] weaponColliders;

        protected override void Awake()
        {
            base.Awake();
            weaponColliders = GetComponentsInChildren<Collider>();
        }

        protected virtual Projectile Shoot()
        {
            var projectileInstance = Instantiate(projectile, transform.position + transform.forward * 0.15f, transform.rotation).GetComponent<Projectile>();
            projectileInstance.properties = projectileProperties;
            var projectileCollider = projectileInstance.GetComponent<Collider>();
            foreach (var weaponCollider in weaponColliders)
                Physics.IgnoreCollision(projectileCollider, weaponCollider);
            return projectileInstance;
        }
    }
}