using System.Linq;
using DG.Tweening;
using Managers;
using Plugins.Renatus.Util.State_Machine;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Entities.Enemies.Dagon_Statue {
    public class DagonStatueAttackState : IDagonState {
        
        #region Fields, Properties, & Constructors 
        
        public DagonStatue DagonStatue { get; set; }

        public DagonStatueAttackState(DagonStatue dagonStatue) {
            DagonStatue = dagonStatue;
        }
        
        #endregion

        #region Interface Functions

        public void Execute() {
        }

        public void Enter() {
        }

        public void Exit() {
        }

        #endregion

        #region Private Functions
        
        
        #endregion
 }
}