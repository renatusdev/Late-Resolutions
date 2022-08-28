using Entities.Player.Behaviors;
using Spear_Gun;
using UnityEngine;
using UnityEngine.InputSystem;

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
        public PlayerInput Input => playerInput;
        
        public float Speed => speed;
        
        // Behaviors
        public PlayerMovement Movement { get; set; }
        public PlayerAim Aim { get; set; }
        
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

        #region Private Functions
    
        private void EnableInput() {
            
            Input.actions["Move"].performed += Movement.GetInput;
            Input.actions["Move"].canceled += Movement.GetInput;
            
            Input.actions["Look"].performed += Aim.GetInput;
            
            // TODO: Abstract this to a IWeapon.
            Input.actions["Aim"].started += spearGun.AimOn;
            Input.actions["Aim"].canceled += spearGun.AimOff;
            Input.actions["Shoot"].performed += spearGun.Shoot;
            Input.actions["Reload"].performed += spearGun.Reload;
        }

        private void DisableInput() {
            Input.actions["Move"].performed += Movement.GetInput;
            Input.actions["Move"].canceled += Movement.GetInput;
            
            Input.actions["Look"].performed -= Aim.GetInput;

            // TODO: Abstract this to a IWeapon.
            Input.actions["Aim"].started -= spearGun.AimOn;
            Input.actions["Aim"].canceled -= spearGun.AimOff;
            Input.actions["Shoot"].performed += spearGun.Shoot;
            Input.actions["Reload"].performed -= spearGun.Reload;
        }

        #endregion
    }
}