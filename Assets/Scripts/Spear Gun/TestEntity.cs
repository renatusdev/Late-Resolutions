using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntity : MonoBehaviour, IShootable {
    
    [SerializeField] private ParticleSystem m_ImpactSubmarineVFX;
    
    public void OnHit(Spear spear, ContactPoint hitPoint) {
        m_ImpactSubmarineVFX.transform.position = hitPoint.point;
        m_ImpactSubmarineVFX.Play();
    }
}
