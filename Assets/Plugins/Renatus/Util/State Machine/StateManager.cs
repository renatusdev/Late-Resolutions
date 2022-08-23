using UnityEngine;

namespace Plugins.Renatus.Util.State_Machine {
	// TODO: Eventually, we might need to change this inheritance driven pattern to more of an object composition approach.
	public abstract class StateManager : MonoBehaviour {
		
		public IBaseState CurrentState { get; protected set; }
		public IBaseState PreviousState { get; protected set; }
		
		public void ChangeState(IBaseState newState) {
			CurrentState?.Exit();

			PreviousState = CurrentState;
			CurrentState = newState;
			
			CurrentState.Enter();
		}

		public bool IsCurrentState(IBaseState state) {
			return CurrentState.Equals(state);
		}
	}
}