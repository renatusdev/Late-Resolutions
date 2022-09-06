using UnityEngine;

namespace Plugins.Renatus.Util.State_Machine {
	// Cannot implement as abstract class because then we would have entities which should inherit MonoBehavior, that
	// would instead inherit this.
	public interface IStateManager {
		
		public IBaseState CurrentState { get; set; }
		public IBaseState PreviousState { get; set; }

		public void ChangeState(IBaseState newState);
		/*
			CurrentState?.Exit();

			PreviousState = CurrentState;
			CurrentState = newState;
			
			CurrentState.Enter(); 
		 */

		public bool IsCurrentState(IBaseState state);
		/*
			return CurrentState.Equals(state);
		 */
	}
}