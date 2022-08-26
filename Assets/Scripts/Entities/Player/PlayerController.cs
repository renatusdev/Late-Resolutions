using System;
using DG.Tweening;
using Spear_Gun;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

// TODO(Sergio): Mouse Sensitivity field from settings.
namespace Entities.Player {
    public class PlayerController : MonoBehaviour, IEntity  {
    
        #region Fields
        
        private const bool MouseInvertedY = false;
        private const float MouseSensitivity = 0.25f;
        private const float SpeedMultiplier = 10;
        
        [Header("Properties")]
        [Range(0, 1)]
        [SerializeField] private float movementSpeed;

        [Header("General References")]
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerInput playerInput;

        [Header("Submarine References")]
        [SerializeField] private SpearGun spearGun;

        public Animator Animator => animator;
        public CharacterController CharacterController => characterController;
        public CameraController CameraController => cameraController;

        /// <summary> The velocity of the player, with a world space direction. </summary>
        public Vector3 Velocity => CharacterController.velocity;
        public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

        private Tweener _lookAtTween;
        private Vector2 _mouseRotation;
        private Vector3 _movementInput;
        private float _rollAxis;

        #endregion

        #region Unity Functions

        private void Start() {
            spearGun.Initialize(this);
            RefreshMouseToRotation();
        }

        private void OnEnable() {
            EnableInput();
        }

        private void OnDisable() {
            DisableInput();
        }

        private void Update() {
            Move(_movementInput);
        }
        
        #endregion

        #region Public Functions

        public void LookAt(Vector3 target, float duration, Ease ease, TweenCallback callback) {
            _lookAtTween ??= transform.DOLookAt(target, duration).SetEase(ease).OnComplete(() => {
                callback?.Invoke();
                RefreshMouseToRotation();
                _lookAtTween = null;
            }); 
        }

        public Vector3 GetPredictedPosition(float timeAhead) {
            return timeAhead * Velocity + transform.position;
        }
        
        /// <summary> Add a direction and force to factor in for this frame of the velocity. </summary>
        public void AddVelocity(Vector3 direction, float force) {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Action Functions

    
        /// <summary>
        /// An event called by the PlayerInput component holding the Move action 2D Axis values.
        /// Note: Since PlayerInput calls this at frames asynchronous from the Update() method,
        /// the multipliers to make velocity framerate independent are calculated in the Move() function
        /// inside the Update method, which runs once per frame. 
        /// </summary>
        public void Move(InputAction.CallbackContext context) {
            Vector2 input = context.ReadValue<Vector2>();
        
            animator.SetBool("isForward", input.y >= 0.7f);
            _movementInput = new Vector3(input.x, 0, input.y);
        }

        public void Move(Vector3 velocity) {
            velocity = transform.TransformDirection(velocity);
            velocity *= movementSpeed * SpeedMultiplier;
            velocity *= Time.deltaTime;
        
            CharacterController.Move(velocity);
        }
    
        private void Look(InputAction.CallbackContext context) {
        
            var delta = context.ReadValue<Vector2>();
        
            delta *= MouseSensitivity;
            delta.y *= (MouseInvertedY ? 1 : -1);

            _mouseRotation.x += delta.x;
            _mouseRotation.y += delta.y;
            _mouseRotation.x %= 360;
            _mouseRotation.y = Mathf.Clamp(_mouseRotation.y, -90, 90);
        
            transform.rotation = Quaternion.Euler(_mouseRotation.y, _mouseRotation.x, 0);
        }
    
        #endregion

        #region Event Functions

        public void OnPause() {
            throw new System.NotImplementedException();
        }

        public void OnUnpause() {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Private Functions
    
        private void EnableInput() {

            playerInput.actions["Move"].performed += Move;
            playerInput.actions["Move"].canceled += Move;
            playerInput.actions["Look"].performed += Look;
            playerInput.actions["Aim"].started += spearGun.AimOn;
            playerInput.actions["Aim"].canceled += spearGun.AimOff;
            playerInput.actions["Shoot"].performed += spearGun.Shoot;
            playerInput.actions["Reload"].performed += spearGun.Reload;
            playerInput.actions["Reload"].performed += spearGun.Reload;
        }

        private void DisableInput() {
            playerInput.actions["Move"].performed -= Move;
            playerInput.actions["Move"].canceled -= Move;
            playerInput.actions["Look"].performed -= Look;
            playerInput.actions["Aim"].started -= spearGun.AimOn;
            playerInput.actions["Aim"].canceled -= spearGun.AimOff;
            playerInput.actions["Reload"].performed -= spearGun.Reload;
        }
    
        private void RefreshMouseToRotation() {
            var rotation = transform.rotation.eulerAngles;
        
            _mouseRotation.x = rotation.y;
            _mouseRotation.y = (rotation.x >= 270) ? rotation.x - 360 : rotation.x;
        }

        #endregion
    }
}