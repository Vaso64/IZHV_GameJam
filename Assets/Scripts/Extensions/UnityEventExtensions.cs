using UnityEngine.Events;

namespace Extensions
{
    public static class UnityEventExtensions
    {
        public static void SetListener(this UnityEvent unityEvent, UnityAction unityAction)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(unityAction);
        }
    }
}