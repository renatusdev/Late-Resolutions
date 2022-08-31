using System.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Entities.Enemies.Dagon_Statue {
    public class DagonStatueAttackState : IDagonState {
        
        #region Fields, Properties, & Constructors 
        
        private float _maxPullDistance;
        private float _maxPullStrength;
        private float _currentPullStrength;
        private float _currentDistance;

        public DagonStatue DagonStatue { get; set; }

        public DagonStatueAttackState(DagonStatue dagonStatue) {
            DagonStatue = dagonStatue;
            _maxPullStrength = DagonStatue.MaxPullStrength;
            _currentPullStrength = _maxPullDistance;
        }
        
        #endregion

        #region Interface Functions

        public void Execute() {
            Pull();
        }

        public void Enter() {
            
            var player = DagonStatue.GameManager.Player;
            var direction = DagonStatue.transform.position - player.transform.position;
            _maxPullDistance = direction.magnitude;
            _currentPullStrength = _maxPullStrength;
            
            DagonStatue.OnAttackFX.Play().AddCameraShake(direction.normalized);
        }

        public void Exit() {
        }
        
        public async void OnHit() {
            await Enrage();
        }

        private async Task Enrage() {
            // FX: Scream, vortex radius shrinks.
            var fx = DagonStatue.OnEnragedFX.Play();
            // Diminish the strength of the pull.
            _currentPullStrength = _maxPullStrength * 0.5f;
            await Task.Delay(1000);
            // FX: Camera shake.
            fx.AddCameraShake((DagonStatue.transform.position - DagonStatue.GameManager.Player.transform.position).normalized);
            await Task.Delay(2000);
            
            // Return pull strength to normal.
            _currentPullStrength = _maxPullStrength;

            // FX??: Enraged, vortex radius and color change. 
        }

        #endregion

        #region Private Functions
        
        private void Pull() {
            var player = DagonStatue.GameManager.Player;
            var direction = DagonStatue.transform.position - player.transform.position;
            _currentDistance = (_maxPullDistance - Mathf.Clamp(direction.magnitude, 0, _maxPullDistance))/_maxPullDistance;
            var magnitude = DagonStatue.PullStrength.Evaluate(_currentDistance) * _currentPullStrength;

            player.Movement.AddExternalVelocity(direction.normalized * magnitude);
        }

        #endregion
 }
}