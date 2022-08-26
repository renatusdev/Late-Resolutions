using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Player.Behaviors {
    /// <summary> Handles player aiming, weapons, and shooting. </summary>
    public class PlayerShooting : MonoBehaviour
    {
        private const float MouseSensitivity = 0.25f;
        private const bool MouseInvertedY = false;
    
        private Vector2 _mouseRotation;

        internal void Initialize() {
            RefreshMouseToRotation();
    
        }
        
        internal void Look(InputAction.CallbackContext context) {
        
            var delta = context.ReadValue<Vector2>();
        
            delta *= MouseSensitivity;
            delta.y *= (MouseInvertedY ? 1 : -1);

            _mouseRotation.x += delta.x;
            _mouseRotation.y += delta.y;
            _mouseRotation.x %= 360;
            _mouseRotation.y = Mathf.Clamp(_mouseRotation.y, -90, 90);
        
            transform.rotation = Quaternion.Euler(_mouseRotation.y, _mouseRotation.x, 0);
        }
        
        internal void RefreshMouseToRotation() {
            var rotation = transform.rotation.eulerAngles;
        
            _mouseRotation.x = rotation.y;
            _mouseRotation.y = (rotation.x >= 270) ? rotation.x - 360 : rotation.x;
        }
    }
}
