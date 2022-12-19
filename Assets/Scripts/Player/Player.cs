using System;
using GameJam.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameJam.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [Range(0, 5)]
        public float maxVelocity = 2f; // m / s
        
        [Range(0, 180)]
        public float rotateSpeed = 90f; // deg / s

        private Rigidbody _rigidbody;
        private Transform _cameraPivot;
        
    
        private void Start()
        {
            // Get references
            _rigidbody = GetComponent<Rigidbody>();
            _cameraPivot = GetComponentInChildren<Camera>().transform;
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
    }
}
