using System;
using System.Collections;
using GameJam.Input;
using UnityEngine;

namespace GameJam.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(Battery))]
    public class Player : MonoBehaviour
    {
        [Range(0, 5)]
        public float maxVelocity = 2f; // m / s
        
        [Range(0, 180)]
        public float rotateSpeed = 90f; // deg / s

        private Rigidbody _rigidbody;
        private Transform _cameraPivot;
        [HideInInspector] public Battery battery;
        
    
        private void Start()
        {
            // Get references
            _rigidbody = GetComponent<Rigidbody>();
            _cameraPivot = GetComponentInChildren<Camera>().transform;
            battery = GetComponent<Battery>();

            StartCoroutine(Oxygen());
        }
    
        private void Update()
        {
            _cameraPivot.localRotation = InputManager.Head.rotation.ReadValue<Quaternion>();
        }

        private void FixedUpdate()
        {
            // Cap speed
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxVelocity);
            
            // Rotate by joystick
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(0, InputManager.Rotate.ReadValue<Vector2>().x * rotateSpeed * Time.fixedDeltaTime, 0));
        }
        
        private IEnumerator Oxygen()
        {
            while (battery.TryDischarge(0.5f))
                yield return new WaitForSeconds(1);
        }
    }
}
