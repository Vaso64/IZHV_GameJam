using System;
using UnityEngine;

namespace GameJam.UI
{
    public class Button3D : UIBase
    {
        [SerializeField] private MeshRenderer buttonMesh;
        
        public Color buttonColor;
        public Color buttonColorPressed;
        public Color buttonColorHover;
        
        protected void Start()
        {
            // Colors
            SetColor(buttonColor);
            OnHoverEnter.AddListener(() => SetColor(buttonColorHover));
            OnHoverExit.AddListener(() => SetColor(buttonColor));
            OnClickUp.AddListener(() => SetColor(buttonColorHover));
            OnClickDown.AddListener(() => SetColor(buttonColorPressed));
        }
        
        private void SetColor(Color color)
        {
            buttonMesh.material.color = color;
            buttonMesh.material.SetColor("_EmissiveColor", color);
        }
    }
}