using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

namespace GameJam.Global
{
    [System.Serializable]
    public struct Level
    {
        public GameObject levelPrefab;
        public string name;
        public Texture image;
        public Cubemap skybox;
    }
}