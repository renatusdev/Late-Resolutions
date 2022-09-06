using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security;
using Managers;
using Plugins.Renatus.Util;
using Plugins.Renatus.Util.FXS;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Debug = UnityEngine.Debug;

public class Spear : MonoBehaviour {

    #region Fields

    private const float c_DEFAULT_VELOCITY = 55;
    private const float c_FADEOUT_WAIT_TIME = 2;
    private const float c_VFX_HITPOINT_OFFSET = 1.25f;

    [Header("General")]
    [SerializeField] private bool m_Debug;
    
    [Header("References")]
    [SerializeField] private AnimationCurve m_VelocityFalloff;
    [SerializeField] private ShaderHelper m_WobbleShader;
    [SerializeField] private string m_LayerOnLaunch;
    [SerializeField] private string m_LayerOnInitialized;
    [SerializeField] private GameObject m_SpearModel;
    
    [Header("FXs")]
    [SerializeField] private ParticleSystem m_TrailVFX;
    [SerializeField] private FXSystem m_ImpactDefaultFX;
    
    public Action OnPushToPool { get; set; }

    private Vector3 m_HitPoint;
    private float m_Velocity;
    private float m_Timer;
    private bool m_IsThrusting;
    private bool m_HasHit;
    private Pool m_Pool;

    #endregion

    #region Public Functions
    
    public void Initialize(Pool pool) {
        SpearReset();
        m_Pool = pool;
    }

    public void Launch(Vector3 hitPoint) {
        Launch(hitPoint, c_DEFAULT_VELOCITY);
    }

    #endregion

    #region Unity Functions
    
    private void Update() {
        if(m_IsThrusting) {
            Thrust();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        
        if(!m_IsThrusting)
            return;
        
        IShootable shootable = collision.collider.GetComponent<IShootable>();
        // Vector3 hitPointOffset = m_HitPoint - (Camera.main.transform.forward * c_VFX_HITPOINT_OFFSET);
        shootable ??= collision.collider.transform.root.GetComponent<IShootable>();
        var hitPoint = collision.GetContact(0).point;
        
        if (shootable == null) {
            m_ImpactDefaultFX.transform.position = hitPoint;
            m_ImpactDefaultFX.Play();
        }
        else {
            shootable.OnHit(this, collision.GetContact(0));
        }

        // When we collide with an object that does not have a 1:1:1 local scale
        // the transformation of the spear scale upon parenting this collider
        // gets scaled incorrectly.
        if (collision.collider.transform.localScale.Equals(Vector3.one)) {
            // Spear should on attach to target (cause target can be destroyed) but
            // rather follow the target position. Currently, a TMP solution was
            // removing the attachment.
            
            // transform.parent = collision.collider.transform;
        }
        else {
            
        }
        OnHit();
    }

    #endregion
    
    #region Private Functions
    
    private void Launch(Vector3 hitPoint, float velocity) {        
        m_HitPoint = hitPoint;
        m_Velocity = velocity;
        m_IsThrusting = true;
        
        m_SpearModel.layer = LayerMask.NameToLayer(m_LayerOnLaunch);
        transform.tag = GameManager.WeaponTag;
        
        transform.SetParent(null);
        transform.LookAt(m_HitPoint);
        
        m_TrailVFX.Play();
    }
    
    private void Thrust() {

        float falloff = m_VelocityFalloff.Evaluate(m_Timer);
        float velocity = m_Velocity * falloff * Time.deltaTime;
        
        if (velocity > 0) {
            transform.Translate(0, 0, velocity, Space.Self);
            m_Timer += Time.deltaTime;
        } else {
            m_IsThrusting = false;
            m_Pool.Enqueue(this.gameObject);
        }
    }
    
    // TODO: What if OnHit, some external script destroys this script? Like how we attach the collided object as the parent,
    // and what if that parent is destroyed immediately after? This is happening when we shoot a ball projectile.
    private void OnHit() {
        if (m_HasHit) return;
        
        m_HasHit = true;
        m_IsThrusting = false;
        
        m_WobbleShader.Play();
        StartCoroutine(CoHitFadeOut());
    }

    private IEnumerator CoHitFadeOut() {
        
        yield return new WaitForSeconds(c_FADEOUT_WAIT_TIME);
        SpearReset();
        m_WobbleShader.Reset();
        m_Pool.Enqueue(this.gameObject);
    }
    
    private void SpearReset() {
        m_IsThrusting = false;
        m_HasHit = false;
        m_Timer = 0;
        m_Velocity = 0;
        
        m_SpearModel.layer = LayerMask.NameToLayer(m_LayerOnInitialized);
        m_ImpactDefaultFX.transform.parent = this.transform;
        
        StopAllCoroutines();
    }

    #endregion
}