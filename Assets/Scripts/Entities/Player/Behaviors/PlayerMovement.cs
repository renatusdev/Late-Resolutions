using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Player {

    /// <summary> Handles player movement. </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Properties")]
        [Range(0, 1)]
        [SerializeField] private float movementSpeed;
        
        [Header("General References")]  [Space(20)]
        [SerializeField] private CharacterController characterController;

        
        private const float SpeedMultiplier = 10;
        private Vector3 _movementInput;
        
        internal void Initialize(Player controller) {
            
        }
        
        /// <summary>
        /// An event called by the PlayerInput component holding the Move action 2D Axis values.
        /// Note: Since PlayerInput calls this at frames asynchronous from the Update() method,
        /// the multipliers to make velocity framerate independent are calculated in the Move() function
        /// inside the Update method, which runs once per frame. 
        /// </summary>
        internal void Move(InputAction.CallbackContext context) {
            Vector2 input = context.ReadValue<Vector2>();
        
            // animator.SetBool("isForward", input.y >= 0.7f);
            _movementInput = new Vector3(input.x, 0, input.y);
        }

        private void Move(Vector3 velocity) {
            velocity = transform.TransformDirection(velocity);
            velocity *= movementSpeed * SpeedMultiplier;
            velocity *= Time.deltaTime;
        
            characterController.Move(velocity);
        }

        #region Public Functions
        
        /// <summary> Returns a predicted player position given the time ahead to predict to. </summary>
        public Vector3 GetPredictedPosition(float timeAhead) {
            return timeAhead * characterController.velocity + transform.position;
        }
        
        /// <summary> Add a direction and force to factor in for this frame of the velocity. </summary>
        public void AddVelocity(Vector3 direction, float force) {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}
