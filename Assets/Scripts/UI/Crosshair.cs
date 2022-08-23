using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Crosshair : MonoBehaviour {

    private static readonly int s_BLOCKED_AIM_ROTATION_MAGNITUDE = 15;
    
    [SerializeField] private Image m_BlockedImage;
    [SerializeField] private Image m_CrosshairImage;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip m_OnAimBlocked;
    
    private AudioSource m_AudioSource;
    private Tween m_BlockedAimRotationTween;

    private void Awake() {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        m_BlockedImage.gameObject.SetActive(false);
        m_CrosshairImage.gameObject.SetActive(true);
    }

    public void RelayBlockedAimCrosshair(float duration) {
        m_CrosshairImage.gameObject.SetActive(false);
        m_BlockedImage.gameObject.SetActive(true);
        
        if (m_BlockedAimRotationTween != null) {
            if (m_BlockedAimRotationTween.IsPlaying()) {
                return;
            }
        }

        m_AudioSource.clip = m_OnAimBlocked;
        m_AudioSource.Play();
        
        m_BlockedAimRotationTween = m_BlockedImage.transform.DOPunchRotation(Vector3.forward * s_BLOCKED_AIM_ROTATION_MAGNITUDE, duration)
        .OnComplete(() => {
            m_BlockedImage.gameObject.SetActive(false);
            m_CrosshairImage.gameObject.SetActive(true);
        });
    }
}
