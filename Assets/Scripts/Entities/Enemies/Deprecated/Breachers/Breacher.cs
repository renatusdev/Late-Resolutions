using Cinemachine;
using DG.Tweening;
using Managers;
using Plugins.Renatus.Util;
using Plugins.Renatus.Util.Events;
using Plugins.Renatus.Util.FXS;
using Plugins.Renatus.Util.State_Machine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

// TODO(Sergio): Investigate Multi-Referential Constraint for spatial awareness.
// https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.1/manual/constraints/MultiReferentialConstraint.html
namespace Entities.Enemies.Breachers {
	public class Breacher : MonoBehaviour, IStateManager, IShootable {

		#region Fields & Properties

		[SerializeField] private Animator animator;
		[SerializeField] private OnTriggerEventHelper triggerToBreach;
		[SerializeField] private ChainIKConstraint chainIKConstraint;
		[SerializeField] private CinemachineImpulseSource impulseSource;
		[SerializeField] private FXSystem breachFX;
		[SerializeField] private FXSystem onHitFX;
		[SerializeField] private Transform target;
		[SerializeField] private GameManager gameManager; // TODO: Remove this dependency.

		public const int StrikeRadius = 5;
		private float _moveDuration;
		
		public BreacherWatchState WatchState { get; private set; }
		public BreacherAttackState AttackState { get; private set; }
		public BreacherDieState DieState { get; private set; }
		
		public OnTriggerEventHelper TriggerToBreach => triggerToBreach;
		public ChainIKConstraint ChainIKConstraint => chainIKConstraint;
		public CinemachineImpulseSource ImpulseSource => impulseSource;
		public GameManager GameManager => gameManager;
		public FXSystem BreachFX => breachFX;
		public Vector3 PlayerPosition => gameManager.Player.transform.position;
		public Transform Target => target;
		public Animator Animator => animator;
		public float Timer { get; private set; }

		#endregion
		
		#region Unity Functions

		public virtual void Update() {
			CurrentState.Execute();
		}

		#endregion

		#region Public Functions

		public virtual void Initialize() {

			WatchState = new BreacherWatchState(this);
			AttackState = new BreacherAttackState(this);
			DieState = new BreacherDieState(this);

			ChangeState(WatchState);
		}

		public void Move() {
			if (Timer >= _moveDuration) 
				return;
			
			Timer += Time.deltaTime;
		}

		public void OnHit(Spear spear, ContactPoint hitPoint) {
			onHitFX.transform.position = hitPoint.point;
			onHitFX.Play();
			
			if (!DieState.IsDying) {
				ChangeState(DieState);
			}
		}

		public Vector3 CreateStrikePosition(Vector3 strikePosition) {
			var position = transform.position;
			var dir = strikePosition - position;
			dir = dir.normalized;
			dir *= StrikeRadius;
			dir += position;
			return dir;
		}
		
		/// <summary>
		/// Rotates the breacher so that the Up vector looks at the target position. 
		/// </summary>
		public void RotateUpTo(Vector3 position, float duration, TweenCallback callback)
		{
			var rotation = Quaternion.LookRotation(position - transform.position);
			rotation = Quaternion.Euler(rotation.eulerAngles.x + 90, rotation.eulerAngles.y, rotation.eulerAngles.z);
			transform.DORotateQuaternion(rotation, duration).OnComplete(callback); 
		}

		public bool IsAnimationCompleted() {
			return Timer >= _moveDuration;
		}

		#endregion
		
		#region Private Functions

		
		#endregion

		public IBaseState CurrentState { get; set; }
		public IBaseState PreviousState { get; set; }
		public void ChangeState(IBaseState newState) {
			CurrentState?.Exit();

			PreviousState = CurrentState;
			CurrentState = newState;
			
			CurrentState.Enter(); 
		}

		public bool IsCurrentState(IBaseState state) {
			return CurrentState.Equals(state);
		}
	}
}

