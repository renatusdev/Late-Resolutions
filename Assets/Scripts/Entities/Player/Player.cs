using System;
using DG.Tweening;
using Entities.Player.Behaviors;
using Spear_Gun;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

// TODO(Sergio): Mouse Sensitivity field from settings.
namespace Entities.Player {
    /// <summary> Handles player input. Initializes movement and shooting. </summary>
    public class Player : MonoBehaviour  {
    
        #region Fields
        
        [Header("General References")]
        [SerializeField] private Animator animator;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerShooting playerShooting;

        [Header("Submarine References")]
        [SerializeField] private SpearGun spearGun;

        public Animator Animator => animator;
        public CameraController CameraController => cameraController;

        private Tweener _lookAtTween;
        private Vector3 _movementInput;
        private float _rollAxis;

        #endregion

        #region Unity Functions

        private void Start() {
            spearGun.Initialize(this);
        }

        private void OnEnable() {
            EnableInput();
        }

        private void OnDisable() {
            DisableInput();
        }

        #endregion

        #region Public Functions

        public void LookAt(Vector3 target, float duration, Ease ease, TweenCallback callback) {
            _lookAtTween ??= transform.DOLookAt(target, duration).SetEase(ease).OnComplete(() => {
                callback?.Invoke();
                playerShooting.RefreshMouseToRotation();
                _lookAtTween = null;
            }); 
        }

        #endregion

        #region Action Functions

        #endregion

        #region Event Functions
        
        public void AimOn() {
            Animator.SetBool("isAiming", true);
            CameraController.SetFOV(75, 0.4f);        
        }
        
        public void AimOff() {
            Animator.SetBool("isAiming", false);
            CameraController.SetFOV(90, 0.2f);
        }

        #endregion

        #region Private Functions
    
        private void EnableInput() {

            playerInput.actions["Move"].performed += playerMovement.Move;
            playerInput.actions["Move"].canceled += playerMovement.Move;
            
            playerInput.actions["Look"].performed += playerShooting.Look;
            
            // TODO: Abstract this to a IWeapon.
            playerInput.actions["Aim"].started += spearGun.AimOn;
            playerInput.actions["Aim"].canceled += spearGun.AimOff;
            playerInput.actions["Shoot"].performed += spearGun.Shoot;
            playerInput.actions["Reload"].performed += spearGun.Reload;
        }

        private void DisableInput() {
            playerInput.actions["Move"].performed += playerMovement.Move;
            playerInput.actions["Move"].canceled += playerMovement.Move;
            
            playerInput.actions["Look"].performed -= playerShooting.Look;

            // TODO: Abstract this to a IWeapon.
            playerInput.actions["Aim"].started -= spearGun.AimOn;
            playerInput.actions["Aim"].canceled -= spearGun.AimOff;
            playerInput.actions["Shoot"].performed += spearGun.Shoot;
            playerInput.actions["Reload"].performed -= spearGun.Reload;
        }

        #endregion
    }
}