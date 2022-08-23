using Plugins.Renatus.Util.State_Machine;
using UnityEngine;

// TODO(Sergio): s_ANIMATION_DURATION should actually be a value obtained from the actual shooting animation.
namespace Spear_Gun {
    public class SpearGunReloadingState : IBaseState {

        private static readonly float s_RELOADING_ANIMATION_DURATION = 1.5f;
    
        private SpearGun m_SpearGun;
        private Pool m_SpearPool;
        private float t;

        public SpearGunReloadingState(SpearGun spearGun, Pool spearPool) {
            m_SpearGun = spearGun;
            m_SpearPool = spearPool;
            t = 0;

            m_SpearPool.Initialize();
        }

        public void Execute() {
            if(IsReloadingDone()) {
                m_SpearGun.ChangeState(m_SpearGun.ReadyState);
            } else {
                t += Time.deltaTime;
            }
        }

        public void Enter() {
            t = 0;
        
            m_SpearGun.PlayerController.Animator.Play("Reloading");
        }

        public void Exit() {
            t = 0;
        }

        public void Reload() {
            Spear spear = m_SpearPool.Dequeue().GetComponent<Spear>();
            spear.Initialize(m_SpearPool);
            m_SpearGun.Spear = spear;
        }

        public bool IsReloadingDone() {
            return t >= s_RELOADING_ANIMATION_DURATION;
        }
    }
}