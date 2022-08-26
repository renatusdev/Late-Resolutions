using Plugins.Renatus.Util.State_Machine;
using UnityEngine;

// Reminder that the in the staggered state the spear can still be attached half way through
namespace Spear_Gun {
    public class SpearGunStaggeredState : IBaseState
    {
        private readonly static float s_STAGGERED_ANIMATION_DURATION = 2;
    
        SpearGun m_SpearGun;
        float t;

        public SpearGunStaggeredState(SpearGun spearGun) {
            m_SpearGun = spearGun;
            t = 0;
        }

        public bool IsCurrentState { get; set; }

        public void Execute() {
            if(IsStaggeredDone()) {
                m_SpearGun.ChangeState(m_SpearGun.ReadyState);
            } else {
                t += Time.deltaTime;
            }
        }

        public void Enter() {
            t = 0;
        
            m_SpearGun.ImpulseSource.GenerateImpulse(Camera.main.transform.forward);
            // Is this no bueno?
            m_SpearGun.Player.Animator.Play("Staggered");
        }

        public void Exit() {
            t = 0;
        }

        public bool IsStaggeredDone() {
            return t >= s_STAGGERED_ANIMATION_DURATION;
        }
    }
}