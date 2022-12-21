using UnityEngine;

namespace GameJam.Items
{
    [System.Serializable]
    public struct ProjectileProperties
    {
        public Color Color;
        public float Velocity;
        public float LifeSpan;
        public Vector3 ShootOffset;
    }
}