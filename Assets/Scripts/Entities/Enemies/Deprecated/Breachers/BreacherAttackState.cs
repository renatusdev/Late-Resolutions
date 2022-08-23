using System;
using DG.Tweening;
using Plugins.Renatus.Util.State_Machine;
using UnityEngine;
using Managers;
using Random = UnityEngine.Random;

// TODO: Time slows.
// TODO: Exit on "breaching animation" complete.
namespace Entities.Enemies.Breachers {
	public class BreacherAttackState : IBaseState {

	#region Fields & Properties

		private const string AnimationStrikeTrigger = "Strike";
		private const float BreachingDuration = 1.5f;
		private const float LooKAtPlayerDuration = 0.5f;
		private const float FocusOnDuration = 0.15f;
		private const float FXActivationTimer = 0.20f;
		private const float PlayerVelocityPredictionMagnitude = 1.4f;

		private readonly Breacher _breacher;
		private bool _hasActivatedFXs;
		private Vector3 _hint;
		
		private bool CanActivateBreachFXs => !_hasActivatedFXs && _breacher.Timer >= FXActivationTimer;

		#endregion
		
		public BreacherAttackState(Breacher breacher) {
			_breacher = breacher;
		}

		public void Execute() {

			_breacher.Move();

			if (CanActivateBreachFXs)
				ActivateFXs();
		}

		public void Enter() {
			_hasActivatedFXs = false;
			
			var anchor = _breacher.transform.position;
			var tip = _breacher.Target.position;
			var playerPredictedPosition = _breacher.GameManager.Player.GetPredictedPosition(PlayerVelocityPredictionMagnitude);
			var strikePositionA = _breacher.CreateStrikePosition(playerPredictedPosition);
			var strikePositionB = (anchor - tip).normalized + anchor;

			// _breacher.RotateUpTo(_breacher.PlayerPosition, LooKAtPlayerDuration, () => {
			// 	_breacher.Animator.SetTrigger(AnimationStrikeTrigger);
			// });
		}

		public void Exit() {
		}

		private void ActivateFXs() {
			_hasActivatedFXs = true;
			// _breacher.GameManager.SlowTime(1); TODO: Reimplement with task.
			_breacher.ImpulseSource.GenerateImpulse();
			_breacher.BreachFX.Play();
			_breacher.GameManager.FocusOn(_breacher.transform.position, FocusOnDuration, Ease.OutBack);
		}
	}
}