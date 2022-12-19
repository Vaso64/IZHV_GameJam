using System;
using UnityEngine;

namespace GameJam.Items
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(Light))]
    public class Projectile : MonoBehaviour
    {
        private ProjectileProperties _properties;
        public ProjectileProperties properties
        {
            get => _properties;
            set
            {
                var material = GetComponent<Renderer>().material;
                material.color = value.Color;
                material.SetColor("_EmissionColor", value.Color);
                GetComponent<Light>().color = value.Color;
                Destroy(gameObject, value.LifeSpan);
                _properties = value;
            }
        }
        
        public event Action<Collider> OnHit;
        
        private Rigidbody _rigidbody;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate() => _rigidbody.velocity = transform.forward * properties.Velocity;

        protected virtual void OnCollisionEnter(Collision collision)
        {
            OnHit?.Invoke(collision.collider);
            Destroy(gameObject);
        } 
    }
}