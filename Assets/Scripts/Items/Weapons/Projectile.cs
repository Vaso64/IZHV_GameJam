using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

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
        private HDAdditionalLightData _light;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _light = GetComponent<HDAdditionalLightData>();
        }

        protected void Start()
        {
            // Muzzle flash
            StartCoroutine(Flash());
        }

        protected virtual void FixedUpdate() => _rigidbody.velocity = transform.forward * properties.Velocity;

        protected virtual void OnCollisionEnter(Collision collision)
        {
            OnHit?.Invoke(collision.collider);
            StartCoroutine(ImpactFlash());
        }

        private IEnumerator ImpactFlash()
        {
            GetComponent<MeshRenderer>().enabled = false;
            yield return StartCoroutine(Flash());
            Destroy(gameObject);
        }

        private IEnumerator Flash()
        {
            const float flashLength = 0.05f;
            const float flashIntensity = 800f;
            var baseIntensity = _light.intensity;
            for(float t = 0; t <= flashLength;)
            {
                t += Time.deltaTime;
                _light.intensity = Mathf.Lerp(flashIntensity, baseIntensity, t / flashLength);
                yield return null;
            }
        }
    }
}