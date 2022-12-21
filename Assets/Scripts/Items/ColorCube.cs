using GameJam.Player;
using UnityEngine;

namespace GameJam.Items
{
    public class ColorCube : GrabItem, IUsablePowered
    {
        public void OnGrab()
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
        
        public void OnRelease()
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
        
        public void Use(Battery battery)
        {
            if (battery.TryDischarge(100f))
                GetComponent<Renderer>().material.color = Random.ColorHSV();
            else
                GetComponent<Renderer>().material.color = Color.red;
        }
    }
}