using Unity.Collections;
using UnityEngine;

namespace Entities.Player.Behaviors {
    public class PlayerRunMovement : IPlayerMoveState {
    
        private const float RunMultiplier = 10;
        private const float SprintMultiplier = 3;
        private const float JumpForce = 2.6f;
        private const float Gravity = -9.82f;
        private const float MaxJumpTime = 0.4f;
        
        public PlayerMovement PlayerMovement { get; set; }
        public Player Player { get; set; }

        private float _fallingTimer;
        private float _jumpTimer;
        private float _runTimer;
        private bool _isJumping;
        
        public PlayerRunMovement(PlayerMovement playerMovement, Player player) {
            PlayerMovement = playerMovement;
            Player = player;
            Player.CameraShakeOnRun.m_ImpulseDefinition.m_ImpulseDuration = Player.MoveSpeed;
        }

        public Vector3 MoveTo() {
            var input = PlayerMovement.XZInput;
            var velocity = new Vector3(input.x, 0, input.y);

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
                    velocity.y += JumpForce * 1-Mathf.Clamp01(_jumpTimer);

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
            
            // TODO: Incorrect use to determine a step.
            if (_runTimer > Player.MoveSpeed) {
                _runTimer = 0;
                OnStep(input);
            }

            if (!input.Equals(Vector3.zero))
                _runTimer += Time.deltaTime + (PlayerMovement.SprintInput * Time.deltaTime);

            // Horizontal speed
            velocity *= Player.MoveSpeed * RunMultiplier + (PlayerMovement.SprintInput * SprintMultiplier);
            velocity *= Time.deltaTime;
            
            return velocity;
        }

        private void OnStep(Vector2 direction) {
            var cameraShake = Player.CameraShakeOnRun;
            
            cameraShake.GenerateImpulseWithVelocity(Vector3.up * (0.15f * -direction.y));   // Camera shake
            cameraShake.m_ImpulseDefinition.m_ImpulseDuration = Player.MoveSpeed; // Refresh duration of shake to player speed.
        }

        public void Enter() {
        }

        public void Exit() {
        }
    }
}