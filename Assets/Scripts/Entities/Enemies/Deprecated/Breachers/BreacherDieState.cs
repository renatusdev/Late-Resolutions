using Plugins.Renatus.Util.State_Machine;
using UnityEngine;

namespace Entities.Enemies.Breachers {
	public class BreacherDieState : IBaseState {
		
		private const float c_DYING_DURATION = 1.15f;
		
		private Breacher m_Breacher;
		public bool IsDying { get; private set; }

		public BreacherDieState(Breacher breacher) {
			m_Breacher = breacher;
			IsDying = false;
		}

		public bool IsCurrentState { get; set; }

		public void Execute() {
			m_Breacher.Move();
		}

		public void Enter() {
			IsDying = true;

			// var end = (Breacher.c_MAX_RADIUS * Random.insideUnitSphere) + m_Breacher.transform.position;
			// var hint = (Breacher.c_MAX_RADIUS * Random.insideUnitSphere) + m_Breacher.Center;
			// end.y -= 5;
			//
			// m_Breacher.SetMoveDuration(c_DYING_DURATION);
			// m_Breacher.SetPath(hint, end);
		}

		public void Exit() {
		}
	}
}