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
        
        private Rigidbody rb;
        private Player player;
        private HandInput handInput;
        private Collider handCollider;
        
        private ConfigurableJoint bodyJoint;
        private ConfigurableJoint grabJoint;
        private readonly List<IGrabbable> grabVicinity = new();
        private readonly List<Collider> staticVicinity = new();
        private IGrabbable grabbedItem;

        private void Start()
        {
            // Get references
            this.bodyJoint = GetComponent<ConfigurableJoint>();
            this.rb = GetComponent<Rigidbody>();
            this.player = GetComponentInParent<Player>();
            this.handCollider = GetComponent<Collider>();
            
            // Get hand input
            switch (handSide)
            {
                case Side.Left:  handInput = InputManager.LeftHand; break;
                case Side.Right: handInput = InputManager.RightHand; break;
            }

            // Register events
            handInput.grab.started += _ => Grab();
            handInput.grab.canceled += _ => Release();
            handInput.use.started += _ => Use();
            handInput.use.canceled += _ => StopUse();
        }   

        private void FixedUpdate()
        {
            // Hand movement
            bodyJoint.targetRotation = handInput.rotation.ReadValue<Quaternion>();
            bodyJoint.targetPosition = handInput.position.ReadValue<Vector3>() - InputManager.Head.position.ReadValue<Vector3>();

            // Boost
            if (handInput.boost.IsPressed() && player.battery.TryDischarge(0.1f))
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
                foreach (var grabbedItemCollider in grabbedItem.gameObject.GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(handCollider, grabbedItemCollider);
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
            if(grabbedItem != null)
            {
                foreach (var grabbedItemCollider in grabbedItem.gameObject.GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(handCollider, grabbedItemCollider, false);
                grabbedItem.OnRelease();
                grabbedItem = null;
            }
        }

        private void Use()
        {
            switch (grabbedItem)
            {
                case IUsable usable: 
                    usable.Use();
                    break;
                case IUsablePowered usablePowered: 
                    usablePowered.Use(player.battery);
                    break;
            }
        }

        private void StopUse()
        {
            switch (grabbedItem)
            {
                case IUsable usable: 
                    usable.StopUse();
                    break;
                case IUsablePowered usablePowered: 
                    usablePowered.StopUse();
                    break;
            }
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