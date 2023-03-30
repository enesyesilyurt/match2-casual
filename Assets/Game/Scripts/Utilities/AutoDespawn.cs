using System;
using System.Collections;
using UnityEngine;

namespace Casual.Utilities
{
    public class AutoDespawn : MonoBehaviour
    {
        [SerializeField] private float time;
        
        private IEnumerator DespawnRoutine()
        {
            yield return new WaitForSeconds(time);
            SimplePool.Despawn(gameObject);
        }

        private void OnEnable()
        {
            StartCoroutine(DespawnRoutine());
        }
    }
}
