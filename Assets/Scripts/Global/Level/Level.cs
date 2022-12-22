using UnityEngine;
using UnityEngine.UI;

namespace GameJam.Global
{
    [System.Serializable]
    public struct Level
    {
        public GameObject levelPrefab;
        public string name;
        public Image image;
    }
}