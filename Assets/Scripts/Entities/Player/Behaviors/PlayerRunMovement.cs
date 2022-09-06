using Unity.Collections;
using UnityEngine;

namespace Entities.Player.Behaviors {
    public class PlayerRunMovement : IPlayerMoveState {
    
        private const float RunMultiplier = 10;
        private const float SprintMultiplier = 3;
        private const float JumpForce = 3;
        private const float Gravity = -9.82f;
        private const float MaxJumpTime = 0.4f;
        
        public PlayerMovement PlayerMovement { get; set; }
        public Player Player { get; set; }

        private float _fallingTimer;
        private float _jumpTimer;
        private bool _isJumping;
        
        public PlayerRunMovement(PlayerMovement playerMovement, Player player) {
            PlayerMovement = playerMovement;
            Player = player;
        }

        public Vector3 MoveTo() {
            var velocity = PlayerMovement.XZInput;

            velocity = Player.transform.TransformDirection(velocity);                      // Convert input to world space direction.
            
            // Vertical logic
            if (!Player.IsGrounded() && !_isJumping) {
                // Gravity Logic
                velocity.y += Gravity * Mathf.Clamp01(_fallingTimer);
                _fallingTimer += Time.deltaTime;
            } else {
                _fallingTimer = 0;
                
                // Jump Logic
                if (PlayerMovement.YInput > 0) {
                    _isJumping = true;
                    velocity.y += JumpForce;

                    if (_jumpTimer > MaxJumpTime) {
                        _isJumping = false;
                        _jumpTimer = 0;
                    }
                    else {
                        _jumpTimer += Time.deltaTime;
                    }
                }
                else {
                    _isJumping = false;
                    _jumpTimer = 0;
                }
            }
            
            // Horizontal speed
            velocity *= Player.MoveSpeed * RunMultiplier + (PlayerMovement.SprintInput * SprintMultiplier);
            velocity *= Time.deltaTime;                                                     // Convert to frame-based velocity.
            
            return velocity;
        }
    
        public void Execute() {
        }

        public void Enter() {
        }

        public void Exit() {
        }
    }
}