using System;
using Cinemachine;
using Managers;
using Plugins.Renatus.Util;
using Plugins.Renatus.Util.Events;
using Plugins.Renatus.Util.State_Machine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Enemies.Dagon_Statue {
    public class DagonStatue : StateManager, IShootable {

        #region Fields & Properties

        [Header("Customizables")]
        [SerializeField] private AnimationCurve pullStrength;
        
        [Header("FXs")]
        [SerializeField] private FXSystem onAttackFX;
        
        [Header("Tools")]
        [SerializeField] private OnTriggerEventHelper triggerEventHelper;
        [SerializeField] private Collider statueCollider;
        [SerializeField] private CinemachineImpulseSource cameraShake;

        public OnTriggerEventHelper OnTriggerEventHelper => triggerEventHelper;
        public AnimationCurve PullStrength => pullStrength;
        public CinemachineImpulseSource CameraShake => cameraShake;
        public Collider StatueCollider => statueCollider;
        public FXSystem OnAttackFX => onAttackFX;

        public DagonStatueWatchState WatchState     { get; private set; }
        public DagonStatueAttackState AttackState   { get; private set; }
        public DagonStatueDieState DieState         { get; private set; }

        public GameManager GameManager { get; private set; }
        public Projectile Projectile { get; set; }

        #endregion
        
        #region Unity Functions

        private void Update() {
            CurrentState?.Execute();
        }

        #endregion
        
        #region Public Functions
        
        public void Initialize(GameManager gameManager) {

            GameManager = gameManager;
            
            WatchState = new DagonStatueWatchState(this);
            AttackState = new DagonStatueAttackState(this);
            DieState = new DagonStatueDieState(this);
            
            ChangeState(WatchState);
        }   
        
        #endregion

        #region Event Functions
        
        public void OnHit(Spear spear, Vector3 hitPoint) {
            if (IsCurrentState(DieState)) return;

            ChangeState(DieState);
        }
        
        #endregion
    }

    public interface IDagonState : IBaseState {
        public DagonStatue DagonStatue { get; set; }
    }
}
