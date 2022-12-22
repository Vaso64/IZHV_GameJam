using System;
using System.Collections.Generic;
using UnityEngine.UI;
using GameJam.Player;
using TMPro;
using UnityEngine;

namespace GameJam.UI
{
    public class BatteryIndicator : MonoBehaviour
    {
        [SerializeField] private Battery battery;
        [SerializeField] private Image batteryFill;
        [SerializeField] private TextMeshPro batteryText;
        
        private float fullFillWidth;

        private void Start() => fullFillWidth = batteryFill.rectTransform.rect.width;

        private void Update()
        {
            var batteryPercentage = battery.currentEnergy / battery.maxEnergy;
            batteryText.text = $"{batteryPercentage:P0}";
            var r = batteryFill.rectTransform.rect;
            batteryFill.color = Color.Lerp(Color.red, Color.green, batteryPercentage);
            batteryFill.rectTransform.sizeDelta = new Vector2(fullFillWidth * batteryPercentage, r.height);
        }
    }
}

