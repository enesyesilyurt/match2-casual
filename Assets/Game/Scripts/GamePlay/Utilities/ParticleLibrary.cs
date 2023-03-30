using System.Collections.Generic;
using Casual.Entities;
using Casual.Enums;
using UnityEngine;

namespace Casual.Utilities
{
    public class ParticleLibrary : MonoSingleton<ParticleLibrary>
    {
        [SerializeField] private ParticleSet[] particleSets;
        
        private Dictionary<Colour, ParticleSystem> particlesDict = new();

        public void Setup()
        {
            for (int i = 0; i < particleSets.Length; i++)
            {
                particlesDict.Add(particleSets[i].Colour, particleSets[i].Particle);
            }
        }

        public ParticleSystem GetParticle(Colour colour)
        {
            return particlesDict[colour];
        }
    }
}
