using GameJam.Common;
using UnityEngine;

namespace GameJam.Items
{
    public interface IGrabbable : IMonoBehaviour
    {
        Rigidbody rigidbody { get; }
        
        void OnGrab(){}
        void OnRelease(){}
    }
}