using Cinemachine;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Plugins.Renatus.Util.FXS {
    
    /// <summary> A manager for FX systems with Audio, Particle Systems, and/or camera shakes. </summary>
    public class FXSystem : MonoBehaviour {
        
        [SerializeField] private new ParticleSystem particleSystem;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private CinemachineImpulseSource cameraShake;
        
        [SerializeField] [HideInInspector] public bool hasVariablePitch;
        [SerializeField] [HideInInspector] public float minimumPitch = 1;
        [SerializeField] [HideInInspector] public float maximumPitch = 1;
        
        public FXSystem Play() {

            if (particleSystem != null) {
                particleSystem.gameObject.SetActive(true);
                particleSystem.Play();
            }

            if (audioSource == null) return this;
            
            if(clips.Length != 0)
                audioSource.clip = clips[Random.Range(0, clips.Length)];
            if(hasVariablePitch)
                audioSource.pitch = Random.Range(minimumPitch, maximumPitch);
            audioSource.Play();

            return this;
        }

        public void AddCameraShake(Vector3 velocity) {
            if(cameraShake != null)
                cameraShake.GenerateImpulseWithVelocity(velocity);
        }
    }

    [CustomEditor(typeof(FXSystem))]
    public class FXSystemEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var fxSystem = target as FXSystem;

            if (fxSystem == null) return;
            
            fxSystem.hasVariablePitch = EditorGUILayout.Toggle("Has Variable Pitch", fxSystem.hasVariablePitch);
            
            if (!fxSystem.hasVariablePitch) return;
            
            fxSystem.minimumPitch = EditorGUILayout.Slider("Minimum Variable Pitch", fxSystem.minimumPitch, -3, 3);
            fxSystem.maximumPitch = EditorGUILayout.Slider("Maximum Variable Pitch", fxSystem.maximumPitch, -3, 3);

            if (GUI.changed) { EditorUtility.SetDirty(fxSystem); }
        }
    }
}
