using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using GUIDebugTool;
using Mono.Cecil;
using Spear_Gun;
using UnityEditor.Animations.Rigging;
using UnityEngine.Animations.Rigging;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

// TODO(Sergio): Mouse Sensitivity field from settings.
[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, IController  {
    
    #region Fields

    private static readonly bool s_MOUSE_INVERTED_Y = false;
    private static readonly float s_MOUSE_SENSITIVITY = 0.25f;
    
    [Header("Properties")]
    [Range(0, 1)]
    [SerializeField] private float m_MovementSpeed;

    [Header("General References")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private CharacterController m_CharacterController;
    [SerializeField] private CameraController m_CameraController;
    [SerializeField] private PlayerInput m_PlayerInput;

    [Header("Submarine References")]
    [SerializeField] private SpearGun m_SpearGun;

    public Animator Animator => m_Animator;
    public CharacterController CharacterController => m_CharacterController;
    public CameraController CameraController => m_CameraController;

    /// <summary> The velocity of the player, with a world space direction. </summary>
    public Vector3 Velocity => CharacterController.velocity;
    public float MovementSpeed { get => m_MovementSpeed; set => m_MovementSpeed = value; }

    private Vector2 m_MouseRotation;
    private Vector3 m_MovementInput;
    private float m_RollAxis;

    #endregion

    #region Unity Functions

    void Awake() {
        InitializeComponents();
    }

    void Start() {
        // TODO(Sergio): Should be done elsewhere. In a game manager.  
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        m_SpearGun.Initialize(this);
        RefreshMouseToRotation();
    }

    void OnEnable() {
        EnableInput();
    }

    void OnDisable() {
        DisableInput();
    }

    void Update() {
        Move(m_MovementInput);
    }

    private void OnDrawGizmos() {
        // Gizmos.DrawSphere(transform.position + (Velocity * 3), 0.5f);
    }

    #endregion

    #region Public Functions
    
    public void LookAt(Transform target, float duration, Ease ease, TweenCallback callback) {
        transform.DOLookAt(target.position, duration).SetEase(ease).OnComplete(() => {
            callback?.Invoke();
            RefreshMouseToRotation();
        });
    }
    
    public void LookAt(Vector3 target, float duration, Ease ease, TweenCallback callback) {
        transform.DOLookAt(target, duration).SetEase(ease).OnComplete(() => {
            callback?.Invoke();
            RefreshMouseToRotation();
        });
    }

    public Vector3 GetPredictedPosition(float timeAhead) {
        return timeAhead * Velocity + transform.position;
    }
    
    #endregion

    #region Action Functions

    /// <summary>
    /// An event called by the PlayerInput component holding the Move action 2D Axis values.
    /// Note: Since PlayerInput calls this at frames asynchronous from the Update() method,
    /// the multipliers to make velocity framerate independent are calculated in the Move() function
    /// inside the Update method, which runs once per frame. 
    /// </summary>
    public void Move(InputAction.CallbackContext context) {
        Vector2 input = context.ReadValue<Vector2>();
        // TODO(Sergio): This is TMP movement animation. Ideally, we should have a blend tree in the animator and here we pass the vector2 as input.
        m_Animator.SetBool("isForward", input.y >= 0.7f);
        m_MovementInput = new Vector3(input.x, 0, input.y);
    }

    public void Move(Vector3 velocity) {
        
        velocity = transform.TransformDirection(velocity);
        velocity *= m_MovementSpeed * 5;
        velocity *= Time.deltaTime;

        CharacterController.Move(velocity);
    }
    
    private void Look(InputAction.CallbackContext context) {
        
        var delta = context.ReadValue<Vector2>();
        
        delta *= s_MOUSE_SENSITIVITY;
        delta.y *= (s_MOUSE_INVERTED_Y ? 1 : -1);

        m_MouseRotation.x += delta.x;
        m_MouseRotation.y += delta.y;
        m_MouseRotation.x %= 360;
        m_MouseRotation.y = Mathf.Clamp(m_MouseRotation.y, -90, 90);
        
        transform.rotation = Quaternion.Euler(m_MouseRotation.y, m_MouseRotation.x, 0);
    }
    
    #endregion

    #region Event Functions

    public void OnPause() {
        throw new System.NotImplementedException();
    }

    public void OnUnpause() {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Private Functions

    private void InitializeComponents() {
        if(m_CharacterController == null) {
            Debug.LogWarning(string.Format("Controller.{0}: Does not have a CharacterController component attached.", gameObject.name));
            if(!TryGetComponent<CharacterController>(out m_CharacterController)) {
                Debug.LogWarning(string.Format("Controller.{0}: Failed to attach CharacterController.", gameObject.name));
            }
        }

        if(m_Animator == null) {
            Debug.LogWarning(string.Format("Controller.{0}: Does not have an Animator component.", gameObject.name));
            if(!TryGetComponent<Animator>(out m_Animator)) {
                Debug.LogWarning(string.Format("Controller.{0}: Failed to attach Animator.", gameObject.name));
            }
        }

        if(m_PlayerInput == null) {
            Debug.LogWarning(string.Format("Controller.{0}: Does not have an m_PlayerInput component.", gameObject.name));
            if(!TryGetComponent<PlayerInput>(out m_PlayerInput)) {
                Debug.LogWarning(string.Format("Controller.{0}: Failed to attach m_PlayerInput.", gameObject.name));
            }
        }

        if(m_SpearGun == null) {
            Debug.LogWarning(string.Format("Controller.{0}: Does not have an m_Gun component.", gameObject.name));
            
            m_SpearGun = GetComponentInChildren<SpearGun>();
            if(m_SpearGun == null) {
                Debug.LogWarning(string.Format("Controller.{0}: Failed to attach m_Gun.", gameObject.name));
            }
        }
    }

    private void EnableInput() {

        m_PlayerInput.actions["Move"].performed += Move;
        m_PlayerInput.actions["Move"].canceled += Move;
        m_PlayerInput.actions["Look"].performed += Look;
        m_PlayerInput.actions["Aim"].started += m_SpearGun.AimOn;
        m_PlayerInput.actions["Aim"].canceled += m_SpearGun.AimOff;
        m_PlayerInput.actions["Shoot"].performed += m_SpearGun.Shoot;
        m_PlayerInput.actions["Reload"].performed += m_SpearGun.Reload;
        m_PlayerInput.actions["Reload"].performed += m_SpearGun.Reload;
    }

    private void DisableInput() {
        m_PlayerInput.actions["Move"].performed -= Move;
        m_PlayerInput.actions["Move"].canceled -= Move;
        m_PlayerInput.actions["Look"].performed -= Look;
        m_PlayerInput.actions["Aim"].started -= m_SpearGun.AimOn;
        m_PlayerInput.actions["Aim"].canceled -= m_SpearGun.AimOff;
        m_PlayerInput.actions["Reload"].performed -= m_SpearGun.Reload;
    }
    
    private void RefreshMouseToRotation() {
        var rotation = transform.rotation.eulerAngles;
        
        m_MouseRotation.x = rotation.y;
        m_MouseRotation.y = (rotation.x >= 270) ? rotation.x - 360 : rotation.x;
    }

    #endregion
}