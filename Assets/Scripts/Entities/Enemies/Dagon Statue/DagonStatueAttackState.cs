using System.Linq;
using DG.Tweening;
using Managers;
using Plugins.Renatus.Util.State_Machine;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem.DualShock;

namespace Entities.Enemies.Dagon_Statue {
    public class DagonStatueAttackState : IDagonState {
        
        #region Fields, Properties, & Constructors 
        
        private const int PullStrength = 8;

        private float _maxPullDistance;
        private float _currentDistance;

        public DagonStatue DagonStatue { get; set; }

        public DagonStatueAttackState(DagonStatue dagonStatue) {
            DagonStatue = dagonStatue;
        }
        
        #endregion

        #region Interface Functions

        public void Execute() {
            Pull();
        }

        public void Enter() {
            // TODO: Awakened Vortex fx (vfx/sfx)
            
            var player = DagonStatue.GameManager.Player;
            var direction = DagonStatue.transform.position - player.transform.position;
            _maxPullDistance = direction.magnitude;

            DagonStatue.CameraShake.GenerateImpulseWithVelocity(direction.normalized);
        }

        public void Exit() {
        }

        #endregion

        #region Private Functions
        
        private void Pull() {
            var player = DagonStatue.GameManager.Player;
            var direction = DagonStatue.transform.position - player.transform.position;
            _currentDistance = (_maxPullDistance - Mathf.Clamp(direction.magnitude, 0, _maxPullDistance))/_maxPullDistance;
            var magnitude = DagonStatue.PullStrength.Evaluate(_currentDistance) * PullStrength;

            player.Movement.AddExternalVelocity(direction.normalized * magnitude);
        }
        
        #endregion
 }
}