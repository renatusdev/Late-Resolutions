using System.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Plugins.Renatus.Util {
    /// TODO(Sergio):
    /// - Audio clip variability support.
    /// - Audio pitch variability support (given a range).
    public class FXSystem : MonoBehaviour {

        [SerializeField] [HideInInspector] public bool m_HasParticleSystem;
        [SerializeField] [HideInInspector] public bool m_HasAudio;
        [SerializeField] [HideInInspector] public bool m_HasMultipleClips;
        [SerializeField] [HideInInspector] public bool m_HasVariablePitch;

        [SerializeField] [HideInInspector] public AudioSource m_AudioSource;
        [SerializeField] [HideInInspector] public ParticleSystem m_ParticleSystem;
        [SerializeField] [HideInInspector] public AudioClip[] m_AudioClips;
        [SerializeField] [HideInInspector] public float m_MinVariablePitch;
        [SerializeField] [HideInInspector] public float m_MaxVariablePitch;

        public void Play() {
            
            if (m_HasParticleSystem) {
                if (m_ParticleSystem == null) {
                    Debug.Log($"FIX: FXSystem for {gameObject.name} does not have a Particle System attached and it does want to use one.");
                }
                m_ParticleSystem.Play();
            }

            if (m_HasAudio) {
                if (m_AudioSource == null) {
                    Debug.Log($"FIX: FXSystem for {gameObject.name} does not have an Audio Source attached and it does want to use one.");
                    return;
                }
                if(m_HasVariablePitch) {
                    m_AudioSource.pitch = Random.Range(m_MinVariablePitch, m_MaxVariablePitch);
                }
                if (m_HasMultipleClips) {
                    m_AudioSource.clip = m_AudioClips[Random.Range(0, m_AudioClips.Length)];
                }
                
                m_AudioSource.Play();
            }
        }
    }
    [CustomEditor(typeof(FXSystem))]
    public class FXSystemEditor : Editor {
        
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            serializedObject.Update();

            FXSystem fxSystem = target as FXSystem;
         
            if(fxSystem == null) return;

            fxSystem.m_HasParticleSystem = EditorGUILayout.Toggle("Has ParticleSystem", fxSystem.m_HasParticleSystem);
            fxSystem.m_HasAudio = EditorGUILayout.Toggle("Has Audio", fxSystem.m_HasAudio);

            if (fxSystem.m_HasParticleSystem)
                fxSystem.m_ParticleSystem = fxSystem.GetComponent<ParticleSystem>();

            if (fxSystem.m_HasAudio) {
                fxSystem.m_AudioSource = fxSystem.GetComponent<AudioSource>();

                if (fxSystem.m_AudioSource == null) {
                    Debug.Log($"FXSystem {fxSystem.name} is asking for AudioSource that is not attached");
                    fxSystem.m_HasAudio = false;
                }
                
                fxSystem.m_HasVariablePitch = EditorGUILayout.Toggle("Has Variable Pitch", fxSystem.m_HasVariablePitch);
                if (fxSystem.m_HasVariablePitch) { 
                    fxSystem.m_MinVariablePitch = EditorGUILayout.Slider("Minimum Variable Pitch", fxSystem.m_MinVariablePitch, -3, 3);
                    fxSystem.m_MaxVariablePitch = EditorGUILayout.Slider("Maximum Variable Pitch", fxSystem.m_MaxVariablePitch, -3, 3);
                }

                fxSystem.m_HasMultipleClips = EditorGUILayout.Toggle("Has Multiple Clips", fxSystem.m_HasMultipleClips);
                if (fxSystem.m_HasMultipleClips) {
                    SerializedProperty audioProperty = serializedObject.FindProperty("m_AudioClips");
                    EditorGUILayout.PropertyField(audioProperty, includeChildren: true);
                    if (audioProperty.hasChildren) {
                        serializedObject.ApplyModifiedProperties();
                    }
                }
            }
            // Variable pitch boolean
            // Render and serialize pitch range
            EditorUtility.SetDirty(fxSystem);
        }
    }
}
