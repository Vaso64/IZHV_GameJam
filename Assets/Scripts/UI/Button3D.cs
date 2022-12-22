using System;
using UnityEngine;

namespace GameJam.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class Button3D : UIBase
    {
        [SerializeField] private MeshRenderer buttonMesh;
        [SerializeField] AudioClip hoverSound;
        [SerializeField] AudioClip clickSound;

        public Color buttonColor;
        public Color buttonColorPressed;
        public Color buttonColorHover;
        
        private AudioSource audioSource;
        
        
        protected void Start()
        {
            audioSource = GetComponent<AudioSource>();
            
            // Colors
            SetColor(buttonColor);
            OnHoverEnter.AddListener(() => SetColor(buttonColorHover));
            OnHoverExit.AddListener(() => SetColor(buttonColor));
            OnClickUp.AddListener(() => SetColor(buttonColorHover));
            OnClickDown.AddListener(() => SetColor(buttonColorPressed));
            OnHoverEnter.AddListener(() => audioSource.PlayOneShot(hoverSound));
            OnClickDown.AddListener(() => audioSource.PlayOneShot(clickSound));
        }
        
        private void SetColor(Color color)
        {
            buttonMesh.material.color = color;
            buttonMesh.material.SetColor("_EmissiveColor", color);
        }
    }
}