using System;
using UnityEngine;
using GameJam.Input;


namespace GameJam.UI
{
    public class UIRayInteractor : MonoBehaviour
    {
        [SerializeField] private Transform ray;
        
        private static UIBase prevHoveredUI;
        private static UIBase currentHoveredUI;

        private static bool isClickDown;
        
        
        private void Start()
        {
            InputManager.RightHand.use.canceled += _ => ClickUp();
            InputManager.RightHand.use.started += _ => ClickDown();
        }

        public void Update()
        {
            var isHit = Physics.Raycast(transform.position, transform.forward, out var hit, Mathf.Infinity);
            var hitIsUi = isHit && hit.transform.CompareTag("UI");

            // Show ray on UI
            ray.localScale = new Vector3(ray.localScale.x, hitIsUi ? Vector3.Distance(hit.point, transform.position) : 0, ray.localScale.z);

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