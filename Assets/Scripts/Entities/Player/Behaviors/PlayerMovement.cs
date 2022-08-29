using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Player.Behaviors {

    public class PlayerMovement
    {
        private const float SpeedMultiplier = 10;
        private const float KickUpMultiplier = 8;
        
        private const float DutchDuration = 0.2f;
        private const int DutchMultiplier = 2;
        private readonly Player _player;
        private Vector3 _xzInput;
        private float _yInput;
        
        public PlayerMovement(Player player) {
            _player = player;
        }

        internal void GetMoveInput(InputAction.CallbackContext context) {
            var input = context.ReadValue<Vector2>();
            
            _player.Animator.SetBool("isForward", input.y >= 0.7f);
            _xzInput = new Vector3(input.x, 0, input.y);
        }

        public void GetKickUpInput(InputAction.CallbackContext obj) {
            _yInput = obj.ReadValue<float>();
        }
        
        internal void Move() {
            
            var velocity = _player.transform.TransformDirection(_xzInput);
            velocity *= _player.MoveSpeed * SpeedMultiplier;
            velocity.y += _yInput * _player.KickUpSpeed * KickUpMultiplier;
            velocity *= Time.deltaTime;
        
            _player.CharacterController.Move(velocity);
            _player.CameraController.SetDutch(-_xzInput.x * DutchMultiplier, DutchDuration);
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
