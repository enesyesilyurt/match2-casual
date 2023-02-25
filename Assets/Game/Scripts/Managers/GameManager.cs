using UnityEngine;
using Casual.Utilities;
using DG.Tweening;
using UnityEngine.Serialization;

namespace Casual.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField, Header("Cube Movement")] private float cubeMaxSpeed = 10f;
        [SerializeField] private float cubeAcceleration = .3f;
        [SerializeField, Header("Anim Settings")] private float specialMergeTime = .3f;
        [SerializeField] private float specialMergeOverShoot = 1.1f;
        [SerializeField] private float shuffleSpeed = .6f;
        [SerializeField, Header("Board Settings")] private float offsetX;
        [SerializeField] private float offsetY;
        [SerializeField, Header("Match Counts")] private int bombMatchCount = 5;
        [SerializeField] private int rocketMatchCount = 8;
        [SerializeField] private int propellerMatchCount = 10;

        public int BombMatchCount => bombMatchCount;
        public int RocketMatchCount => rocketMatchCount;
        public int PropellerMatchCount => propellerMatchCount;
        public float CubeMaxSpeed => cubeMaxSpeed;
        public float ShuffleSpeed => shuffleSpeed;
        public float SpecialMergeTime => specialMergeTime;
        public float SpecialMergeOverShoot => specialMergeOverShoot;
        public float CubeAcceleration => cubeAcceleration;
        public float OffsetX => offsetX;
        public float OffsetY => offsetY;
        
        private void Awake()
        {
            DOTween.Init();
            DOTween.SetTweensCapacity(300,100);
            
            ImageLibrary.Instance.Setup();
            ParticleLibrary.Instance.Setup();
            ItemFactory.Instance.Setup();
            LevelManager.Instance.Setup();
            ScreenManager.Instance.Setup();
        }
    }
}
