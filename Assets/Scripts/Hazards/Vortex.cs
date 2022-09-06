using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Hazards {

    public class Vortex : MonoBehaviour {

        [SerializeField] private Material vortexMaterial;
        [SerializeField] private CinemachineImpulseSource cameraShake;
        [SerializeField] private ParticleSystem vfx;
        [SerializeField] private AudioSource jumpScareSound;
        [SerializeField] private AudioSource enragedSound;
        [SerializeField] private AudioSource tensionSound;
        [SerializeField] private AudioSource hurtSound;

        [SerializeField] private Color normalColor;
        [SerializeField] private Color enragedColor;

        public Color NormalColor => normalColor;
        public Color EnragedColor => enragedColor;
        
        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private static readonly int PrimaryColor = Shader.PropertyToID("_Color");

        private bool _isRecoiled;

        private void Awake() {
            vortexMaterial.SetFloat(Radius, 0);
            vortexMaterial.SetColor(PrimaryColor,normalColor);
            _isRecoiled = false;
        }

        public void Play(Vector3 cameraDirection) {
            // Resets
            gameObject.SetActive(true);
            tensionSound.volume = 0;
            vortexMaterial.SetFloat(Radius, 0);
            
            vortexMaterial.DOFloat(1, Radius, 1.5f).SetEase(Ease.OutElastic);
            cameraShake.GenerateImpulseWithVelocity(cameraDirection);
            // vfx.Play();
            jumpScareSound.Play();
            tensionSound.Play();
        }

        public void Hurt() {
            _isRecoiled = true;
            hurtSound.Play();
            jumpScareSound.DOFade(0.15f, 0.3f);
            tensionSound.DOFade(0.25f, 0.3f);
            // vfx.Stop();
            vortexMaterial.DOFloat(0.3f, Radius, 0.6f).SetEase(Ease.InOutElastic);
        }

        public void Enrage() {
            _isRecoiled = false;
            tensionSound.DOFade(1, 0.2f);
            // vfx.Play();
            vortexMaterial.DOColor(enragedColor, 0.5f);
            cameraShake.GenerateImpulse();
            enragedSound.Play();
            vortexMaterial.DOFloat(1, Radius, 0.5f).SetEase(Ease.InOutElastic);
        }

        public void SetDistanceToPlayer(float distance) {
            if(_isRecoiled) return;
            
            tensionSound.volume = Mathf.Lerp(tensionSound.volume, Mathf.Clamp01(distance + 0.2f), Time.deltaTime);
        }
    }
}
