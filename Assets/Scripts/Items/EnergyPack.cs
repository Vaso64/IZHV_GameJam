using GameJam.Player;
using UnityEngine;

namespace GameJam.Items
{
    public class EnergyPack : GrabItem, IUsablePowered
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
            energyBallIndicator.GetComponent<Renderer>().material.SetFloat("_EmissiveIntensity", 1);
            used = true;
        }
    }
}