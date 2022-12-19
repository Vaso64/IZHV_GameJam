using System;
using System.Collections.Generic;
using System.Linq;
using GameJam.Common;
using GameJam.Input;
using GameJam.Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJam.Player
{
    [RequireComponent(typeof(ConfigurableJoint))]
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] private Side handSide;
        
        private ConfigurableJoint bodyJoint;
        private ConfigurableJoint grabJoint;
        private Rigidbody rb;
        private HandInput handInput;
        
        private readonly List<IGrabbable> grabVicinity = new();
        private readonly List<Collider> staticVicinity = new();
        private IGrabbable grabbedItem;

        private void Start()
        {
            // Get references
            this.bodyJoint = GetComponent<ConfigurableJoint>();
            this.rb = GetComponent<Rigidbody>();
            switch (handSide)
            {
                case Side.Left:  handInput = InputManager.LeftHand; break;
                case Side.Right: handInput = InputManager.RightHand; break;
            }

            // Register events
            handInput.grab.started += _ => Grab();
            handInput.grab.canceled += _ => Release();
            handInput.use.started += _ =>
            {
                if (grabbedItem is IUsable usable) 
                    usable.Use();
            };
            handInput.use.canceled += _ =>
            {
                if (grabbedItem is IUsable usable) 
                    usable.StopUse();
            };
        }   

        private void FixedUpdate()
        {
            // Hand movement
            bodyJoint.targetRotation = handInput.rotation.ReadValue<Quaternion>();
            bodyJoint.targetPosition = handInput.position.ReadValue<Vector3>() - InputManager.Head.position.ReadValue<Vector3>();

            // Boost
            if (handInput.boost.IsPressed())
                rb.AddForce(transform.forward * 10);
        }
        
        private void Grab()
        {
            Release();
            
            // Grab item
            if (grabVicinity.Any())
            {
                grabbedItem = grabVicinity.First();
                grabJoint = grabbedItem.gameObject.AddComponent<ConfigurableJoint>();
                grabJoint.swapBodies = true;
                grabJoint.connectedBody = rb;
                grabJoint.massScale = 1/100f;
                grabbedItem.OnGrab();
            }

            // Grab static
            else if (staticVicinity.Any())
                grabJoint = gameObject.AddComponent<ConfigurableJoint>();

            // Nothing to grab
            else
                return;
            
            grabJoint.xMotion = ConfigurableJointMotion.Locked;
            grabJoint.yMotion = ConfigurableJointMotion.Locked;
            grabJoint.zMotion = ConfigurableJointMotion.Locked;
            grabJoint.angularXMotion = ConfigurableJointMotion.Locked;
            grabJoint.angularYMotion = ConfigurableJointMotion.Locked;
            grabJoint.angularZMotion = ConfigurableJointMotion.Locked;
        }

        private void Release()
        {
            if (grabJoint == null) 
                return;
            
            // Destroy joint
            Destroy(grabJoint);
            grabJoint = null;

            // Notify grabbed item
            grabbedItem?.OnRelease();
            grabbedItem = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IGrabbable grabbable))
                grabVicinity.Add(grabbable);
            else if (other.gameObject.isStatic)
                staticVicinity.Add(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IGrabbable grabbable))
                grabVicinity.Remove(grabbable);
            else if (other.gameObject.isStatic)
                staticVicinity.Remove(other);
        }
    }
}