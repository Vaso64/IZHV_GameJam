using GameJam.Items;
using UnityEngine;

namespace GameJam.AI
{
    public class EnemyWeapon : Weapon
    {
        public float damage = 50f;
        
        protected override void Awake()
        {
            base.Awake();
            OnHit += OnHitCallback;
        }
        
        private void OnHitCallback(Collider other)
        {
            var player = other.GetComponentInParent<Player.Player>();
            if(player != null)
                player.battery.TryDischarge(damage);
        }
    }
}