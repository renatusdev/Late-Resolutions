using Cinemachine;
using Entities.Player;
using Plugins.Renatus.Util;
using Plugins.Renatus.Util.State_Machine;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO(Sergio): .Shoot(): Only shoot in shootable areas (or pointing farrr away), else, relay "x" icon in non-shootable. (Based off of raycast).
namespace Spear_Gun {
    public class SpearGun : StateManager  {

        #region Fields

        [Space(5)] [Header("General")]
        [SerializeField] private bool m_Debug;
        [SerializeField] private Pool m_SpearPool;
        [SerializeField] private CinemachineImpulseSource m_ImpulseSource;
        [SerializeField] private Crosshair m_Crosshair;
    
        [Space(5)] [Header("FXs")]
        [SerializeField] private FXSystem m_MuzzleShotFX;
        [SerializeField] private FXSystem m_BubblesFX;
    
        // private BaseState m_PreviousState;
        // private BaseState m_CurrentState;
    
        public Spear Spear { get; set; }
    
        public SpearGunReadyState ReadyState { get; private set; }
        public SpearGunReloadingState ReloadingState { get; private set; }
        public SpearGunShootingState ShootingState { get; private set; }
        public SpearGunStaggeredState StaggeredState { get; private set; }

        public PlayerController PlayerController { get; private set; }
        public CinemachineImpulseSource ImpulseSource => m_ImpulseSource;
        public Crosshair CrossHair => m_Crosshair;

        #endregion

        #region Unity Functions

        private void Update() {
            CurrentState?.Execute();
        }

        private void OnDrawGizmosSelected() {
            if (!m_Debug) return;

            Gizmos.DrawSphere(ShootingState.Hitpoint, 0.25f);
        
            Ray forwardRayFromCameraCenter = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Debug.DrawRay(forwardRayFromCameraCenter.origin + (Camera.main.transform.forward * 2), forwardRayFromCameraCenter.direction, Color.red);
        }

        #endregion

        #region Public Functions

        public void Initialize(PlayerController playerController) {
            PlayerController = playerController;
        
            ReadyState = new SpearGunReadyState(this);
            ReloadingState = new SpearGunReloadingState(this, m_SpearPool);
            ShootingState = new SpearGunShootingState(this, m_BubblesFX, m_MuzzleShotFX);
            StaggeredState = new SpearGunStaggeredState(this);

            ChangeState(ReadyState);

            Spear = m_SpearPool.Dequeue().GetComponent<Spear>();
            Spear.Initialize(m_SpearPool);
        }

        public bool HasSpear() {
            return Spear != null;
        }

        #endregion

        #region Event Functions

        /// <summary>
        /// Event triggered by Speargun reloading animation.
        /// It assumes we are in the reload state (where the reloading animation is triggered).
        /// </summary>
        public void OnSpearAttached() {
            ReloadingState.Reload();
        }

        #endregion
    
        #region Action Functions
    
        internal void Shoot(InputAction.CallbackContext context) {
            if(HasSpear() && CurrentState.Equals(ReadyState)) {
                ChangeState(ShootingState);
            }
        }

        internal void Reload(InputAction.CallbackContext context) {
            if (!HasSpear() && CurrentState.Equals(ReadyState)) {
                ChangeState(ReloadingState);
            } else {
                // TODO(Sergio): SpearGun.Reload(): TODO: Relay the fact that the spear is already loaded.");
            }   
        }
    
        internal void AimOn(InputAction.CallbackContext obj) {
            if(!CurrentState.Equals(ReadyState))
                return;

            ReadyState.AimOn();
        }

        internal void AimOff(InputAction.CallbackContext obj) {
            if(!CurrentState.Equals(ReadyState))
                return;

            ReadyState.AimOff();
        }
    
        #endregion
    }
}