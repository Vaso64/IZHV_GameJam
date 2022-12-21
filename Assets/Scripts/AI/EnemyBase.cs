using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameJam.AI
{
    public abstract class EnemyBase : MonoBehaviour
    {
        protected Dictionary<EnemyStateType, (float minLevel, float maxLevel, Func<IEnumerator> routine)> StateList;
        protected (float level, EnemyStateType type, Coroutine routine) CurrentState;
        
        [Range(0,100)]
        [SerializeField] protected float awareness;
        
        [Range(0,100)]
        [SerializeField] protected float memory;
    }
}


