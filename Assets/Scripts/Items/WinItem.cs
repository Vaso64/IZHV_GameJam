using GameJam.Global;
using GameJam.Player;
using UnityEngine;

namespace GameJam.Items
{
    public class WinItem : GrabItem, IUsablePowered
    {
        public bool used { get; private set; } = false;
        
        public void Use(Battery battery)
        {
            if(used)
                return;
            
            used = true;

            StartCoroutine(GlobalReferences.GameLoop.Win());
        }
    }
}