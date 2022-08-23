namespace Plugins.Renatus.Util.State_Machine {
    public interface IBaseState {

        public void Execute();
        public void Enter();
        public void Exit();
    }
}