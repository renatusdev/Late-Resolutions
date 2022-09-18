using UnityEngine;

namespace Entities.Player.Behaviors {
    public class PlayerSwimMovement : IPlayerMoveState {
        
        private const float SwimSpeedMultiplier = 10;
        private const float SwimUpMultiplier = 8;
        
        public PlayerMovement PlayerMovement { get; set; }
        public Player Player { get; set; }

        public PlayerSwimMovement(PlayerMovement playerMovement, Player player) {
            PlayerMovement = playerMovement;
            Player = player;
        }
        
        public Vector3 MoveTo() {
            
            var velocity = new Vector3(PlayerMovement.XZInput.x, 0, PlayerMovement.XZInput.y);
            
            velocity = Player.transform.TransformDirection(velocity);                      // Convert input to world space direction.
            velocity *= Player.MoveSpeed * SwimSpeedMultiplier;                            // Add speed.
            velocity.y += PlayerMovement.YInput * Player.JumpForce * SwimUpMultiplier;     // Add kicking up to float speed.
            velocity *= Time.deltaTime;                                                     // Convert to frame-based velocity.
            
            return velocity;
        }

        public void Enter() {
        }

        public void Exit() {
        }
    }
}