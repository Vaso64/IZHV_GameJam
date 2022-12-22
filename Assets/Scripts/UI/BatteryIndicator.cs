using System;
using System.Collections.Generic;
using GameJam.Player;
using TMPro;
using UnityEngine;

namespace GameJam.UI
{
    [RequireComponent(typeof(TextMeshPro))]
    public class BatteryIndicator : MonoBehaviour
    {
        [SerializeField] private Battery battery;
        private TextMeshPro textIndicator;

        private void Start()
        {
            textIndicator = GetComponent<TextMeshPro>();
        }

        private void Update()
        {
            textIndicator.text = $"{battery.currentEnergy:F} / {battery.maxEnergy}";
        }
    }
}

