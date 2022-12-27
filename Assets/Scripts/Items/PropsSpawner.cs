using System.Collections.Generic;
using GameJam.Extensions;
using UnityEngine;

namespace GameJam.Items
{
    [RequireComponent(typeof(Collider))]
    public class PropsSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> props;
        [SerializeField] private int propsCount;
    
        private void Start()
        {
            var collider = GetComponent<Collider>();
            collider.isTrigger = true;
            for (var i = 0; i < propsCount; i++)
            {
                var propInstance = Instantiate(props[Random.Range(0, props.Count)], collider.bounds.RandomPoint(), Random.rotation, transform);
                propInstance.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
            }
            
        }
    }
}
