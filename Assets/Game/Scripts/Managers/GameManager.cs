using System;
using UnityEngine;
using Casual.Utilities;
using DG.Tweening;
using NaughtyAttributes;
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
        [SerializeField, Header("Match Counts")] private int propellerMatchCount = 6;
        [SerializeField, Header("Objects")] private GameObject border;

        public GameObject Border => border;
        public int PropellerMatchCount => propellerMatchCount;
        public float CubeMaxSpeed => cubeMaxSpeed;
        public float ShuffleSpeed => shuffleSpeed;
        public float SpecialMergeTime => specialMergeTime;
        public float SpecialMergeOverShoot => specialMergeOverShoot;
        public float CubeAcceleration => cubeAcceleration;
        public float OffsetX => offsetX;
        public float OffsetY => offsetY;

        private GameState currentGameState;

        public event Action<GameState> GameStateChanged;
        
        private void Awake()
        {
            DOTween.Init();
            DOTween.SetTweensCapacity(300,100);
            
            ImageLibrary.Instance.Setup();
            ParticleLibrary.Instance.Setup();
            CollectibleManager.Instance.Setup();
            LevelManager.Instance.Setup();
            ScreenManager.Instance.Setup();
            UIManager.Instance.Setup();
            ChangeGameState(GameState.Home);
        }

        public void ChangeGameState(GameState newState)
        {
            currentGameState = newState;
            
            GameStateChanged?.Invoke(newState);
        }
    }
}
