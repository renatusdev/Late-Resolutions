using Plugins.Renatus.Util.State_Machine;
using UnityEngine;

namespace Entities.Enemies.Dagon_Statue {
    public class DagonStatueDieState : IDagonState {
        
        public DagonStatue DagonStatue { get; set; }

        public DagonStatueDieState(DagonStatue dagonStatue) {
            DagonStatue = dagonStatue;
        }

        public void Execute() {
        }

        public void Enter() {
            Debug.Log("Dead boi.");
        }

        public void Exit() {
        }
        
        public void OnHit() {
            
        }

    }
}