using System;
using DG.Tweening;
using Entities.Player.Behaviors;
using Spear_Gun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

// TODO(Sergio): Mouse Sensitivity field from settings.
namespace Entities.Player {
    /// <summary> Handles player input. Initializes movement and shooting. </summary>
    public class Player : MonoBehaviour  {
    
        #region Fields
        
        [Header("Properties")]
        [Range(0, 1)]
        [SerializeField] private float speed;
        
        [Header("General References")]
        [SerializeField] private Animator animator;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private CharacterController characterController;
        
        [Header("???")]
        [SerializeField] private SpearGun spearGun;

        // Component & Fields References
        public CharacterController CharacterController => characterController;
        public CameraController CameraController => cameraController;
        public Animator Animator => animator;
        public float Speed => speed;
        
        // Behaviors
        public PlayerMovement Movement { get; set; }
        public PlayerAim Aim { get; set; }
        
        private Tweener _lookAtTween;

        #endregion

        #region Unity Functions

        private void Start() {
            spearGun.Initialize(this);
            
            Movement = new PlayerMovement(this);
            Aim = new PlayerAim(this);
            
            EnableInput();
        }
        
        private void OnDisable() {
            DisableInput();
        }

        private void Update() {
            Movement.Move();
            Aim.Look();
        }

        #endregion

        #region Public Functions

        public void LookAt(Vector3 target, float duration, Ease ease, TweenCallback callback) {
            _lookAtTween ??= transform.DOLookAt(target, duration).SetEase(ease).OnComplete(() => {
                callback?.Invoke();
                // Shooting.RefreshMouseToRotation(); // TODO: This should not go here right?!?!?!??!?!
                _lookAtTween = null;
            }); 
        }

        #endregion
        
        #region Private Functions
    
        private void EnableInput() {
            
            playerInput.actions["Move"].performed += Movement.GetInput;
            playerInput.actions["Move"].canceled += Movement.GetInput;
            
            playerInput.actions["Look"].performed += Aim.GetInput;
            
            // TODO: Abstract this to a IWeapon.
            playerInput.actions["Aim"].started += spearGun.AimOn;
            playerInput.actions["Aim"].canceled += spearGun.AimOff;
            playerInput.actions["Shoot"].performed += spearGun.Shoot;
            playerInput.actions["Reload"].performed += spearGun.Reload;
        }

        private void DisableInput() {
            playerInput.actions["Move"].performed += Movement.GetInput;
            playerInput.actions["Move"].canceled += Movement.GetInput;
            
            playerInput.actions["Look"].performed -= Aim.GetInput;

            // TODO: Abstract this to a IWeapon.
            playerInput.actions["Aim"].started -= spearGun.AimOn;
            playerInput.actions["Aim"].canceled -= spearGun.AimOff;
            playerInput.actions["Shoot"].performed += spearGun.Shoot;
            playerInput.actions["Reload"].performed -= spearGun.Reload;
        }

        #endregion
    }
}