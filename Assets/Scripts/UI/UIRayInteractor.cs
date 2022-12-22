using System;
using UnityEngine;
using GameJam.Input;


namespace GameJam.UI
{
    [RequireComponent(typeof(LineRenderer))]
    public class UIRayInteractor : MonoBehaviour
    {
        private static UIBase prevHoveredUI;
        private static UIBase currentHoveredUI;
        private static LineRenderer line;

        private static bool isClickDown;
        
        private void Start()
        {
            InputManager.RightHand.use.canceled += _ => ClickUp();
            InputManager.RightHand.use.started += _ => ClickDown();
            line = GetComponent<LineRenderer>();
        }

        public void Update()
        {
            var isHit = Physics.Raycast(transform.position, transform.forward, out var hit, Mathf.Infinity);
            var hitIsUi = isHit && hit.transform.CompareTag("UI");

            // Show ray on UI
            line.SetPosition(0, transform.position);
            line.SetPosition(1, hitIsUi ? hit.point: transform.position);

            if (!isClickDown)
                currentHoveredUI = hitIsUi ? hit.transform.GetComponentInParent<UIBase>() : null;
                
            if (prevHoveredUI != currentHoveredUI)
            {
                if(prevHoveredUI != null)
                    prevHoveredUI.OnHoverExit.Invoke();
                
                if(currentHoveredUI != null)
                    currentHoveredUI.OnHoverEnter.Invoke();
            }
            
            prevHoveredUI = currentHoveredUI;
        }

        private static void ClickDown()
        {
            isClickDown = true;
            if(currentHoveredUI != null)
                currentHoveredUI.OnClickDown?.Invoke();
        }
        
        private static void ClickUp()
        {
            isClickDown = false;
            if(currentHoveredUI != null)
                currentHoveredUI.OnClickUp?.Invoke();
        }
    }
}