using System;
using Cinemachine;
using Entities.Player.Behaviors;
using Spear_Gun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Jobs;

// TODO(Sergio): Mouse Sensitivity field from settings.
namespace Entities.Player {
    /// <summary> Handles player input. Initializes movement and shooting. </summary>
    public class Player : MonoBehaviour  {
    
        #region Fields
        
        [Header("Properties")]
        
        [Range(0, 1)] [SerializeField] private float moveSpeed;
        [Range(0, 1)] [SerializeField] private float jumpForce;
        
        [Header("References")]
        [SerializeField] private GameObject toolHolder;
        [SerializeField] private SpearGun spearGun;
        
        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private CinemachineImpulseSource cameraShakeOnRun;        

        // Component & Fields References
        public CharacterController CharacterController => characterController;
        public CameraController CameraController => cameraController;
        public Animator Animator => animator;
        public PlayerInput Input => playerInput;
        public CinemachineImpulseSource CameraShakeOnRun => cameraShakeOnRun;
        
        public float MoveSpeed => moveSpeed;
        public float JumpForce => jumpForce;
        
        public PlayerMovement Movement { get; set; }
        public PlayerAim Aim { get; set; }
        public PlayerTool Tool { get; set; }

        public GameObject ToolHolder => toolHolder;
        
        #endregion

        #region Unity Functions

        private void Start() {
            // TODO: TMP
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            // TODO: Remove this. It should exist in the PlayerTool script.
            spearGun.Initialize(this);
            
            Movement = new PlayerMovement(this);
            Aim = new PlayerAim(this);
            Tool = new PlayerTool(this);

            Tool.Clear();
            EnableInput();
        }
        
        private void OnDisable() {
            DisableInput();
        }

        private void Update() {
            Movement?.Move();
            Aim?.Look();
            
            IsGrounded();
        }

        #endregion

        #region Public Functions

        public bool IsGrounded() {
            
            var ray = new Ray(CharacterController.height * 0.5f * Vector3.down + transform.position, Vector3.down);
            return Physics.Raycast(ray, out var hit, CharacterController.skinWidth);
        }

        #endregion

        #region Private Functions
    
        private void EnableInput() {
            // TODO: Have a class which handles the add/remove of all these inputs.

            Input.actions["Move"].performed += Movement.GetMoveInput;
            Input.actions["Move"].canceled += Movement.GetMoveInput;
            Input.actions["Jump"].performed += Movement.GetJumpInput;
            Input.actions["Jump"].canceled += Movement.GetJumpInput;
            Input.actions["Sprint"].performed += Movement.GetSprintInput;
            Input.actions["Sprint"].canceled += Movement.GetSprintInput;

            
            Input.actions["Look"].performed += Aim.GetInput;
            
            // TODO: Abstract this to a IWeapon.
            Input.actions["Aim"].started += spearGun.AimOn;
            Input.actions["Aim"].canceled += spearGun.AimOff;
            Input.actions["Shoot"].performed += spearGun.Shoot;
            Input.actions["Reload"].performed += spearGun.Reload;
        }

        private void DisableInput() {
            Input.actions["Move"].performed -= Movement.GetMoveInput;
            Input.actions["Move"].canceled -= Movement.GetMoveInput;
            Input.actions["Jump"].performed -= Movement.GetJumpInput;
            Input.actions["Jump"].canceled -= Movement.GetJumpInput;
            Input.actions["Sprint"].performed -= Movement.GetSprintInput;
            Input.actions["Sprint"].canceled -= Movement.GetSprintInput;

            
            Input.actions["Look"].performed -= Aim.GetInput;

            // TODO: Abstract this to a IWeapon.
            Input.actions["Aim"].started -= spearGun.AimOn;
            Input.actions["Aim"].canceled -= spearGun.AimOff;
            Input.actions["Shoot"].performed -= spearGun.Shoot;
            Input.actions["Reload"].performed -= spearGun.Reload;
        }
        
        #endregion
    }
}