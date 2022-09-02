using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEngine;

public class ShaderHelper : MonoBehaviour {

    [Serializable]
    public class PropertyHelper {
        
        [SerializeField] private string m_Name;
        [SerializeField] private bool m_IsLoop;
        [SerializeField] private float m_Duration;
        [SerializeField] private float m_To;
        [SerializeField] private bool m_HasInheretedFrom;
        [SerializeField] [HideInInspector] private float m_From;

        protected float m_Timer;
        private float m_OriginalFrom;
        
        public bool HasInheretedFrom => m_HasInheretedFrom;
        public float From { get => m_From; set => m_From = value;  }
        
        public void Update(Material material) {
            if (m_Timer < m_Duration) {
                material.SetFloat(m_Name, Mathf.Lerp(m_From, m_To, m_Timer / m_Duration));
                m_Timer += Time.deltaTime;
            } else {
                material.SetFloat(m_Name, m_To);
            
                if (m_IsLoop) {
                    m_Timer = 0;
                } else {
                    // enabled = false;
                }
            }
        }

        public void Initialize(Material material) {
            m_OriginalFrom = material.GetFloat(m_Name);
            
            if (m_HasInheretedFrom) {
                m_From = m_OriginalFrom;
            }
            m_Timer = 0;
        }

        public void Reset(Material material) {
            
            material.SetFloat(m_Name, m_OriginalFrom);
            m_Timer = 0;
        }
    }

    [SerializeField] private bool m_OnStart;
    [SerializeField] private PropertyHelper[] m_Properties;
    
    public PropertyHelper[] Properties => m_Properties;
    
    private Material m_Material;
    private Renderer m_Renderer;
    private bool m_IsPlaying;

    private void Awake() {
        m_Renderer = GetComponent<Renderer>();
        m_Material = m_Renderer.material;
    }

    private void Start() {
        if (m_Material == null)
            enabled = false;
        
        if(m_OnStart)
            Play();
    }

    private void Update() {

        if (!m_IsPlaying) return;
        
        foreach (PropertyHelper property in m_Properties) {
            property.Update(m_Material);
        }
    }
    
    public void Play() {
        foreach (PropertyHelper property in m_Properties) {
            property.Initialize(m_Material);
        }

        m_IsPlaying = true;
    }

    public void Reset() {
        m_IsPlaying = false;
        
        foreach (PropertyHelper property in m_Properties) {
            property.Reset(m_Material);
        }
    }
}