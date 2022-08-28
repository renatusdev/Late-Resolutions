using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Player.Behaviors {

    public class PlayerMovement
    {
        private const float SpeedMultiplier = 10;
        private readonly Player _player;
        private Vector3 _movementInput;
        
        public PlayerMovement(Player player) {
            _player = player;
        }

        /// <summary>
        /// An event called by the PlayerInput component holding the Move action 2D Axis values.
        /// Note: Since PlayerInput calls this at frames asynchronous from the Update() method,
        /// the multipliers to make velocity framerate independent are calculated in the Move() function
        /// inside the Update method, which runs once per frame. 
        /// </summary>
        public void Move(InputAction.CallbackContext context) {
            // var input = context.ReadValue<Vector2>();
            //
            // _player.Animator.SetBool("isForward", input.y >= 0.7f);
            // _movementInput = new Vector3(input.x, 0, input.y);
        }

        private void Move(Vector3 velocity) {
            velocity = _player.transform.TransformDirection(velocity);
            velocity *= _player.Speed * SpeedMultiplier;
            velocity *= Time.deltaTime;
        
            _player.CharacterController.Move(velocity);
        }

        #region Public Functions
        
        /// <summary> Returns a predicted player position given the time ahead to predict to. </summary>
        public Vector3 GetPredictedPosition(float timeAhead) {
            return timeAhead * _player.CharacterController.velocity + _player.transform.position;
        }
        
        /// <summary> Add a direction and force to factor in for this frame of the velocity. </summary>
        public void AddVelocity(Vector3 direction, float force) {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
