using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Casual
{
    public class MainLayoutsUIController : MonoBehaviour
    {
        [SerializeField] private HomeUIController homeUIController;

        public void Initialize()
        {
            homeUIController.Initialize();
        }
    }
}
