using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Player.Behaviors {
    /// <summary> Handles player aiming, weapons, and shooting. </summary>
    public class PlayerAim
    {
        private const float MouseSensitivity = 0.1f;
        private const bool MouseInvertedY = false;
    
        private Vector2 _mouseRotation;
        private Vector3 _inputMouseDelta;
        
        private readonly Player _player;
        
        public PlayerAim(Player player) {
            _player = player;
            RefreshMouseToRotation();
        }
        
        internal void GetInput(InputAction.CallbackContext context) {
            _inputMouseDelta = context.ReadValue<Vector2>();
        }

        internal void Look() {
            _inputMouseDelta *= MouseSensitivity;
            _inputMouseDelta.y *= (MouseInvertedY ? 1 : -1);

            _mouseRotation.x += _inputMouseDelta.x;
            _mouseRotation.y += _inputMouseDelta.y;
            _mouseRotation.x %= 360;
            _mouseRotation.y = Mathf.Clamp(_mouseRotation.y, -90, 90);
        
            _player.transform.rotation = Quaternion.Euler(_mouseRotation.y, _mouseRotation.x, 0);
        }

        public void AimOn() {
            _player.Animator.SetBool("isAiming", true);
            _player.CameraController.SetFOV(75, 0.4f);        
        }

        public void AimOff() {
            _player.Animator.SetBool("isAiming", false);
            _player.CameraController.SetFOV(90, 0.2f);
        }
        
        internal void RefreshMouseToRotation() {
            var rotation = _player.transform.rotation.eulerAngles;
        
            _mouseRotation.x = rotation.y;
            _mouseRotation.y = (rotation.x >= 270) ? rotation.x - 360 : rotation.x;
        }
    }
}
