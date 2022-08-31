using Cinemachine;
using Managers;
using Plugins.Renatus.Util;
using Plugins.Renatus.Util.Events;
using Plugins.Renatus.Util.FXS;
using Plugins.Renatus.Util.State_Machine;
using UnityEngine;

namespace Entities.Enemies.Dagon_Statue {
    public class DagonStatue : StateManager, IShootable {

        #region Fields & Properties

        [Header("Customizables")]
        [SerializeField] [Range(0, 20)] private float maxPullStrength;
        [SerializeField] private AnimationCurve pullStrengthToDistance;
        
        [Header("FXs")]
        [SerializeField] private FXSystem onAttackFX;
        [SerializeField] private FXSystem onEnragedFX;
        
        [Header("Tools")]
        [SerializeField] private OnTriggerEventHelper triggerEventHelper;
        [SerializeField] private Collider statueCollider;

        public OnTriggerEventHelper OnTriggerEventHelper => triggerEventHelper;
        public AnimationCurve PullStrength => pullStrengthToDistance;
        public Collider StatueCollider => statueCollider;
        public FXSystem OnAttackFX => onAttackFX;
        public FXSystem OnEnragedFX => onEnragedFX;
        public float MaxPullStrength => maxPullStrength;

        public DagonStatueWatchState WatchState     { get; private set; }
        public DagonStatueAttackState AttackState   { get; private set; }
        public DagonStatueDieState DieState         { get; private set; }

        public GameManager GameManager { get; private set; }
        public Projectile Projectile { get; set; }

        private int _health;

        #endregion
        
        #region Unity Functions

        private void Update() {
            
            // TODO: This should NOT crash! (it does when Update() is called before Initialize().)
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
            _health = 2;
        }   
        
        #endregion

        #region Event Functions
        
        public void OnHit(Spear spear, Vector3 hitPoint) {
            if (_health <= 0) return;
            _health--;
            
            if (_health <= 0) {
                ChangeState(DieState);
                return;
            }
            
            (CurrentState as IDagonState)?.OnHit();
        }
        
        #endregion
    }

    public interface IDagonState : IBaseState {
        public DagonStatue DagonStatue { get; set; }

        public void OnHit();
    }
}
