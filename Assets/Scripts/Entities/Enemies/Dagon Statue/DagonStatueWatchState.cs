using GUIDebugTool;
using Managers;
using Plugins.Renatus.Util.State_Machine;
using UnityEngine;

namespace Entities.Enemies.Dagon_Statue {
    public class DagonStatueWatchState : IDagonState {

        public DagonStatue DagonStatue { get; set; }

        public DagonStatueWatchState(DagonStatue dagonStatue) {
            DagonStatue = dagonStatue;
            DagonStatue.OnTriggerEventHelper.Initialize(OnPlayerEnterTrigger);
        }

        private void OnPlayerEnterTrigger(Collider other) {
            if (!DagonStatue.IsCurrentState(this)) return;          // Is not the current state.
            if (!other.CompareTag(GameManager.PlayerTag)) return;   // Is not the player.

            DagonStatue.ChangeState(DagonStatue.AttackState);
        }
        
        public void Execute() {
        }

        public void Enter() {
        }

        public void Exit() {
        }
        
        public void OnHit() {
            // TODO: Change to attack state and in enraged!
        }
    }
}