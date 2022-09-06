using System.Data;
using Plugins.Renatus.Util.State_Machine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

namespace Entities.Player.Behaviors {

    public class PlayerMovement : IStateManager
    {
        public IBaseState CurrentState { get; set; }
        public IBaseState PreviousState { get; set; }
        
        private const float DutchDuration = 0.2f;
        private const int DutchMultiplier = 2;
        private Vector3 _externalVelocity;

        public PlayerSwimMovement PlayerSwimMovement { get; set; }
        public PlayerRunMovement PlayerRunMovement { get; set; }
        
        public Player Player { get; private set; }
        public Vector3 XZInput { get; private set; }
        public float YInput { get; private set; }
        public float SprintInput { get; private set; }
        
        public PlayerMovement(Player player) {
            Player = player;

            PlayerSwimMovement = new PlayerSwimMovement(this, Player);
            PlayerRunMovement = new PlayerRunMovement(this, Player);
            
            ChangeState(PlayerRunMovement);
        }

        internal void GetMoveInput(InputAction.CallbackContext move) {
            var input = move.ReadValue<Vector2>();
            
            XZInput = new Vector3(input.x, 0, input.y);
            Player.Animator.SetBool("isForward", input.y >= 0.7f);
        }

        public void GetJumpInput(InputAction.CallbackContext jump) {
            YInput = jump.ReadValue<float>();
        }
        
        public void GetSprintInput(InputAction.CallbackContext sprint) {
            SprintInput = sprint.ReadValue<float>();
        }
        
        internal void Move() {

            var velocity = (CurrentState as IPlayerMoveState).MoveTo();

            velocity += _externalVelocity * Time.deltaTime; // Factor in any external velocity.
        
            Player.CharacterController.Move(velocity);
            Player.CameraController.SetDutch(-XZInput.x * DutchMultiplier, DutchDuration);

            _externalVelocity = Vector3.zero;
        }

        #region Public Functions
        
        /// <summary> Returns a predicted player position given the time ahead to predict to. </summary>
        public Vector3 GetPredictedPosition(float timeAhead) {
            return timeAhead * Player.CharacterController.velocity + Player.transform.position;
        }

        /// <summary> Add a magnitude and direction to factor in for this frame of the velocity. </summary>
        public void AddExternalVelocity(Vector3 velocity) {
            _externalVelocity = velocity;
        }
        
        public void ChangeState(IBaseState newState) {
            CurrentState?.Exit();

            PreviousState = CurrentState;
            CurrentState = newState;
			
            CurrentState.Enter();         
        }

        public bool IsCurrentState(IBaseState state) {
            return CurrentState.Equals(state);
        }
        
        #endregion
        
    }
    
    public interface IPlayerMoveState : IBaseState {
        public PlayerMovement PlayerMovement { get; set; }
        public Player Player { get; set; }
        
        public Vector3 MoveTo();
    }
}