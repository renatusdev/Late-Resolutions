using System.Threading.Tasks;
using DG.Tweening;
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
            DagonStatue.Vortex.SetDistanceToPlayer(_currentDistance);
            DagonStatue.transform.LookAt(DagonStatue.GameManager.Player.transform);
            
            Debug.Log(_currentPullStrength);
        }

        public void Enter() {
            
            var player = DagonStatue.GameManager.Player;
            var direction = DagonStatue.transform.position - player.transform.position;
            _maxPullDistance = direction.magnitude;
            _currentPullStrength = _maxPullStrength;
            
            DagonStatue.Vortex.Play(direction.normalized);
            DagonStatue.GameManager.Player.Aim.FocusOn(DagonStatue.transform.position, 0.75f, Ease.OutBack);
            DagonStatue.GameManager.Overlay.PlayFlash(DagonStatue.Vortex.NormalColor, 0.4f, Ease.Flash, Ease.Flash);
        }

        public void Exit() {
        }
        
        public void OnHit() {
            Enrage();
        }

        private async void Enrage() {

            DagonStatue.Vortex.Hurt();
            _currentPullStrength = _maxPullStrength * 0.5f;

            await Task.Delay(2000);

            DagonStatue.Vortex.Enrage();
            _currentPullStrength = _maxPullStrength;
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