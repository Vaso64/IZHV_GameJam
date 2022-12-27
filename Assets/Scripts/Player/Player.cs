using System;
using System.Collections;
using GameJam.Global;
using GameJam.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;

namespace GameJam.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(Battery))]
    public class Player : MonoBehaviour
    {
        [Range(0, 10)] public float maxVelocity = 2f; // m / s

        [Range(0, 180)] public float rotateSpeed = 90f; // deg / s
        
        public float passiveDrain = 0.5f;
        
        private Transform _cameraPivot;
        
        [HideInInspector] public Battery battery;
        [HideInInspector] public Rigidbody rb;
        
        private Vector3 prevHmdPos;

        private void Awake()
        {
            // Get references
            rb = GetComponent<Rigidbody>();
            _cameraPivot = GetComponentInChildren<Camera>().transform;
            battery = GetComponent<Battery>();
        }


        private void Start()
        {
            // Dont collide with self
            var playerColliders = GetComponentsInChildren<Collider>();
            foreach (var playerColliderA in playerColliders)
            foreach (var playerColliderB in playerColliders)
                if(playerColliderA != playerColliderB)
                    Physics.IgnoreCollision(playerColliderA, playerColliderB);

            StartCoroutine(PassiveDrain());
        }

        private void Update()
        {
            _cameraPivot.localRotation = InputManager.Head.rotation.ReadValue<Quaternion>();
        }
        
        public void SetRoomScaleMovement(Transform pivot = null)
        {
            rb.isKinematic = pivot != null;
            transform.SetParent(pivot);
            if(pivot != null) 
                transform.localRotation = Quaternion.identity;
        }

        private void FixedUpdate()
        {
            // Cap speed
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

            // Rotate by joystick
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0,
                InputManager.Rotate.ReadValue<Vector2>().x * rotateSpeed * Time.fixedDeltaTime, 0));

            // Move by HMD
            var hmdPos = InputManager.Head.position.ReadValue<Vector3>();
            if (rb.isKinematic) transform.localPosition = hmdPos; // Absolute movement
            else rb.MovePosition(rb.position + rb.rotation * (hmdPos - prevHmdPos)); // Delta movement
            prevHmdPos = hmdPos;
        }

        private IEnumerator PassiveDrain()
        {
            while (true)
            {
                if (battery.TryDischarge(passiveDrain))
                    yield return new WaitForSeconds(1f);
                else
                    yield return StartCoroutine(OutOfEnergy());
            }
                
        }
        
        private IEnumerator OutOfEnergy()
        {
            battery.Discharge(passiveDrain);
            var sequenceLength = 5f;
            var time = 0f;
            FindObjectOfType<Volume>().sharedProfile.TryGet<ColorAdjustments>(out var colorAdjustments); 
            while (!battery.TryDischarge(0.5f))
            {
                // Slowly fade to grayscale
                colorAdjustments.saturation.value = Mathf.Lerp(0, -100, time / sequenceLength);
                time += Time.deltaTime;
                if (time > sequenceLength)
                {
                    StartCoroutine(GlobalReferences.GameLoop.GameOver());
                    yield break;
                }
            }
        }
    }
}
