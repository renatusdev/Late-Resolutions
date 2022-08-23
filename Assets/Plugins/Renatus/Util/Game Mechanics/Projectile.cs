using System;
using Plugins.Renatus.Util;
using UnityEngine;
using UnityEngine.Scripting;

public class Projectile : MonoBehaviour {

    [Header("Properties")]
    [SerializeField] private float speed;
    [SerializeField] private Collider projectileCollider;

    [Header("Optional FXs")]
    [SerializeField] private FXSystem muzzleFx;
    [SerializeField] private FXSystem trailFx;
    [SerializeField] private FXSystem impactFX;

    private Action<Collision> _onHit;
    private Action _onMissed;
    private bool _isPropelling;
    private float _timer;

    #region Unity Functions

    private void Update() {
        if (!_isPropelling) return;

        Propel();
        _timer += Time.deltaTime;
    }

    #endregion

    #region Public Functions

    /// <summary> Initializes a bullet, adding its event on collision, and on the case that the bullet flew too far. An optional parameter
    /// is also what collider should this projectile ignore. </summary>
    public void Initialize(Action<Collision> onHit, Action onMissed) {
        _onHit += onHit;
        _onMissed += onMissed;
        _isPropelling = false;
        _timer = 0;
    }

    public void IgnoreCollision(Collider toIgnore) {
        Physics.IgnoreCollision(toIgnore, projectileCollider);
    }

    /// <summary> Projectile looks at the target, sets its parent to null, activates the muzzle and trial fxs if they exist, and allows itself to propel forward. </summary>
    public void Shoot(Vector3 target) {
        Shoot(target, speed);
    }

    /// <summary> Projectile looks at the target, sets its parent to null, activates the muzzle and trial fxs if they exist, and allows itself to propel forward. </summary>
    public void Shoot(Vector3 target, float speed) {
        this.speed = speed;
        transform.SetParent(null);
        transform.LookAt(target);

        if (muzzleFx != null) muzzleFx.Play();
        if (trailFx != null) trailFx.Play();

        _isPropelling = true;
    }

    #endregion

    #region Event Functions

    private void OnCollisionEnter(Collision collision) {
        
        // Do not register collisions if the projectile is not propelling (remains idle in muzzle).
        if(!_isPropelling) return;

        if (impactFX != null) {
            impactFX.transform.SetParent(null);
            impactFX.transform.position = collision.GetContact(0).point;
            impactFX.Play();
        }

        _timer = 0;
        _isPropelling = false;
        _onHit?.Invoke(collision);
    }

    #endregion

    #region Private Functions
    
    private void Propel() {
        
        if (_timer < 3) {
            transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
        } else {
            _timer = 0;
            _isPropelling = false;
            _onMissed?.Invoke();
        }
    }
    
    #endregion
    
}