using UnityEngine;
using UnityEngine.Events;

namespace GameJam.UI
{
    public class UIBase : MonoBehaviour {
        public UnityEvent OnClickUp;
        
        public UnityEvent OnClickDown;
        
        public UnityEvent OnHoverEnter;
        
        public UnityEvent OnHoverExit;
    }
}