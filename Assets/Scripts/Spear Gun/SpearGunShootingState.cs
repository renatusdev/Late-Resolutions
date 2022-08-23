using Plugins.Renatus.Util;
using Plugins.Renatus.Util.State_Machine;
using UnityEngine;

// TODO(Sergio): s_ANIMATION_DURATION should actually be a value obtained from the actual shooting animation.
// TODO(Sergio): Enter(): Needs TAG! and we add a static of NONSHOOTABLE TAGS. If yes, then no shoot, if no shoot to shootable target or void with (default impact vfx / case specific prob).
namespace Spear_Gun {
    public class SpearGunShootingState : IBaseState {
    
        private static readonly float s_SHOOTING_ANIMATION_DURATION = 0.4f;
        private static readonly string s_NONSHOOTABLE_LAYER = "Nonshootable";

        private SpearGun m_SpearGun;
        private FXSystem m_BubblesFX;
        private FXSystem m_MuzzleShotFX;
        private Camera m_Camera;
        private float t;

        public Vector3 Hitpoint { get; private set; }

        public SpearGunShootingState(SpearGun spearGun, FXSystem bubbles, FXSystem muzzleShot) {
            m_SpearGun = spearGun;
            m_BubblesFX = bubbles;
            m_MuzzleShotFX = muzzleShot;
            m_Camera = Camera.main;
            t = 0;
        }

        public void Execute() {
            if(IsShootingDone()) {
                m_SpearGun.ChangeState(m_SpearGun.ReadyState);
            } else {
                t += Time.deltaTime;
            }
        }

        public void Enter() {
            t = 0;
        
            Ray forwardRayFromCameraCenter = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Physics.Raycast(forwardRayFromCameraCenter, out RaycastHit hit);
        
            if (hit.collider != null) {
                if (hit.collider.tag.Equals(s_NONSHOOTABLE_LAYER)) {
                    m_SpearGun.CrossHair.RelayBlockedAimCrosshair(0.75f);
                    m_SpearGun.ChangeState(m_SpearGun.ReadyState);
                    return;
                }
                else {
                    Hitpoint = hit.point;
                    m_SpearGun.Spear.Launch(hit.point);
                }
            }
            else {
                Vector3 emptySpaceDirection = forwardRayFromCameraCenter.origin + m_Camera.transform.forward * 50;
                m_SpearGun.Spear.Launch(emptySpaceDirection);
            }
        
            m_SpearGun.Spear = null;
            m_SpearGun.ImpulseSource.GenerateImpulse(m_Camera.transform.forward);
            m_BubblesFX.Play();
            m_MuzzleShotFX.Play();
        }

        public void Exit() {
            t = 0;
        }

        public bool IsShootingDone() {
            return t >= s_SHOOTING_ANIMATION_DURATION;
        }
    }
}