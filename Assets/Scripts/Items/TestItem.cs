using UnityEngine;

namespace GameJam.Items
{
    public class TestItem : ItemBase, IUsable
    {
        public void OnGrab()
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        
        public void OnRelease()
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
        
        public void Use()
        {
            GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
        
        public void StopUse()
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
    }
}