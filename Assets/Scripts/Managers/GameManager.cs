using System;
using System.Threading.Tasks;
using DG.Tweening;
using Entities.Enemies.Dagon_Statue;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers {
	public class GameManager : MonoBehaviour {

		#region Fields & Properties
		
		public static readonly string PlayerTag = "Player";
		public static readonly string WeaponTag = "Weapon";
		
		[SerializeField] private PlayerController player;
		[SerializeField] private CameraController cameraController;


		/// TODO: Eventually, we will revisit this to contemplate a better approach for collecting/spawning entities.
		/// Because ideally, the entities should not even be spawned.
		[Header("Enemy Entities")]
		[SerializeField] private DagonStatue dagonStatue;
		
		public PlayerController Player => player;
		public bool HasFocusOn { get; private set; }

		#endregion
		
		
		#region Unity Functions

		private void Awake() {
			dagonStatue.Initialize(this);
		}

		#endregion

		#region Public Functions
		
		public void FocusOn(Transform target, float duration, Ease ease, TweenCallback callback = null) {
			if (HasFocusOn) return;
			
			HasFocusOn = true;
			player.LookAt(target, duration, ease, () => {
				HasFocusOn = false;
				callback?.Invoke();
			});
		}
		
		public void FocusOn(Vector3 target, float duration, Ease ease, TweenCallback callback = null) {
			if (HasFocusOn) return;
			
			HasFocusOn = true;
			player.LookAt(target, duration, ease, () => {
				HasFocusOn = false;
				callback?.Invoke();
			});
		}
		
		public void ForceFocusOn(Transform target, float duration, Ease ease, TweenCallback callback = null) {
			if (HasFocusOn) {
				// Add functionality to cancel previous focus on, then force this new one.
				// I think FOV script has tween storing/reusing.
			}
			
			player.LookAt(target, duration, ease, () => {
				HasFocusOn = false;
				callback?.Invoke();
			});
		}

		#endregion

		#region Private Functions

		#endregion
		
	}
}
