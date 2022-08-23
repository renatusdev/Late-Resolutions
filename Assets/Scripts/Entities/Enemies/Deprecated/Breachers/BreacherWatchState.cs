using System.Runtime.Remoting.Services;
using Managers;
using Plugins.Renatus.Util.Events;
using Plugins.Renatus.Util.State_Machine;
using UnityEngine;

namespace Entities.Enemies.Breachers {
	public class BreacherWatchState : IBaseState {

		private const string c_PLAYER_TAG = "Player";
		private Breacher m_Breacher;

		public void Execute() {
		}

		public void Enter() {
		}

		public void Exit() {
		}

		public BreacherWatchState(Breacher breacher) {
			m_Breacher = breacher;
			m_Breacher.TriggerToBreach.Initialize(OnPlayerEnterTrigger);
		}
		
		private void OnPlayerEnterTrigger(Collider other) {
			if (!m_Breacher.IsCurrentState(this)) return;
			if (!other.CompareTag(GameManager.PlayerTag)) return;
			
			m_Breacher.ChangeState(m_Breacher.AttackState);
		}
	}
}