using System;
using Casual.Enums;
using UnityEngine;

namespace Casual.Entities
{
    [Serializable]
    public class Row
    {
        [SerializeField] private Colour[] colours;

        public Colour[] Colours => colours;
    }
}
