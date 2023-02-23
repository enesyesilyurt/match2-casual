using System.Collections;
using UnityEngine;

namespace Casual.Utilities
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] private float time;
        
        private void Start()
        {
            Destroy(gameObject, time);
        }
    }
}
