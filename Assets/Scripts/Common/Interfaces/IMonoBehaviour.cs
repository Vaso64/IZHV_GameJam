using UnityEngine;

namespace GameJam.Common
{
    public interface IMonoBehaviour
    {
        Transform transform { get; }
        GameObject gameObject { get; }
    }
}