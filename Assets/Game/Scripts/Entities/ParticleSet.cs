using System;
using Casual.Enums;
using UnityEngine;

namespace Casual.Entities
{
    [Serializable]
    public class ParticleSet
    {
        [SerializeField] private Colour colour;
        [SerializeField] private ParticleSystem particle;

        public Colour Colour => colour;
        public ParticleSystem Particle => particle;
    }
}
