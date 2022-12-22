using System;
using System.Collections;
using GameJam.Global;
using GameJam.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace GameJam.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(Battery))]
    public class Player : MonoBehaviour
    {
        [Range(0, 5)] public float maxVelocity = 2f; // m / s

        [Range(0, 180)] public float rotateSpeed = 90f; // deg / s
        
        public float passiveDrain = 0.5f;
        

        private Rigidbody _rigidbody;
        private Transform _cameraPivot;
        [HideInInspector] public Battery battery;

        private Vector3 prevHmdPos;


        private void Start()
        {
            // Get references
            _rigidbody = GetComponent<Rigidbody>();
            _cameraPivot = GetComponentInChildren<Camera>().transform;
            battery = GetComponent<Battery>();

            StartCoroutine(PassiveDrain());
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
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(0,
                InputManager.Rotate.ReadValue<Vector2>().x * rotateSpeed * Time.fixedDeltaTime, 0));

            // Move by HMD
            var hmdPos = InputManager.Head.position.ReadValue<Vector3>();
            if (_rigidbody.isKinematic) transform.localPosition = hmdPos; // Absolute movement
            else _rigidbody.MovePosition(_rigidbody.position + _rigidbody.rotation * (hmdPos - prevHmdPos)); // Delta movement
            prevHmdPos = hmdPos;
        }

        public void SetRoomScaleMovement(Transform pivot = null)
        {
            _rigidbody.isKinematic = pivot != null;
            transform.SetParent(pivot);
            if(pivot != null) 
                transform.localRotation = Quaternion.identity;
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
