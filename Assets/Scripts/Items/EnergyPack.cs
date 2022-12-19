using GameJam.Player;
using UnityEngine;

namespace GameJam.Items
{
    public class EnergyPack : ItemBase, IUsablePowered
    {
        public float EnergyAmount;

        [SerializeField] private GameObject energyBallIndicator;
        
        public bool used { get; private set; } = false;
        
        public void Use(Battery battery)
        {
            if(used)
                return;
            
            battery.Charge(EnergyAmount);   
            energyBallIndicator.GetComponent<Light>().enabled = false;
            energyBallIndicator.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            used = true;
        }
    }
}