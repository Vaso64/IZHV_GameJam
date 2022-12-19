using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJam.Input
{
    public class InputManager : MonoBehaviour
    {
        // Readouts
        public static HeadInput Head;
        public static HandInput LeftHand;
        public static HandInput RightHand;
        public static InputAction Rotate;
        
        private PlayerInputAction.VRPlayerActions vrActions;
        
        private void Awake()
        {
            // Setup action
            var playerInputAction = new PlayerInputAction();
            playerInputAction.Enable();
            vrActions = playerInputAction.VRPlayer;
            
            // Setup input field
            Head = new HeadInput
            {
                position = vrActions.HMDPosition,
                rotation = vrActions.HMDRotation
            };

            LeftHand = new HandInput
            {
                position = vrActions.LeftControllerPosition,
                rotation = vrActions.LeftControllerRotation,
                use = vrActions.LeftControllerUse,
                grab = vrActions.LeftControllerGrab,
                boost = vrActions.LeftControllerBoost
            };
            
            RightHand = new HandInput
            {
                position = vrActions.RightControllerPosition,
                rotation = vrActions.RightControllerRotation,
                use = vrActions.RightControllerUse,
                grab = vrActions.RightControllerGrab,
                boost = vrActions.RightControllerBoost
            };
            
            Rotate = vrActions.Rotate;

        }

        private void Update()
        {
            InputSystem.Update();
        }
    }
}