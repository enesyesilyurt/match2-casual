using System;
using System.Collections;
using UnityEngine;

namespace Casual.Utilities
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] private float time;
        
        private IEnumerator DestroyRoutine()
        {
            yield return new WaitForSeconds(time);
            SimplePool.Despawn(gameObject);
        }

        private void OnEnable()
        {
            StartCoroutine(DestroyRoutine());
        }
    }
}
