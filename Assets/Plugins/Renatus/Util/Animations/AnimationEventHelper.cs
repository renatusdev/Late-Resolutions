using UnityEngine;
using UnityEngine.Events;

namespace Plugins.Renatus.Util {
    [RequireComponent(typeof(Animator))]
    public class AnimationEventHelper : MonoBehaviour {
    
        public UnityEvent m_OnAnimationTrigger;

        public void Trigger() {
            m_OnAnimationTrigger.Invoke();
        }
    }
}
