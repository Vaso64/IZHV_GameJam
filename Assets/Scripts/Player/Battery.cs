using System;
using UnityEngine;

namespace GameJam.Player
{
    public class Battery : MonoBehaviour
    {
        public float currentEnergy { get; private set; }
        public float currentPercent => currentEnergy / maxEnergy;
        public float maxEnergy;


        private void Awake()
        {
            currentEnergy = maxEnergy;
        }

        public void Charge(float amount)
        {
            currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        }
        
        public bool TryDischarge(float amount)
        {
            if (!(currentEnergy >= amount)) 
                return false;
            
            currentEnergy -= amount;
            return true;
        }
        
        public void Discharge(float amount)
        {
            currentEnergy = Mathf.Clamp(currentEnergy - amount, 0, maxEnergy);
        }
    }
}