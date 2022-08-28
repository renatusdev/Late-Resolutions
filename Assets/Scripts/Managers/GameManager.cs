using System;
using System.Threading.Tasks;
using DG.Tweening;
using Entities.Enemies.Dagon_Statue;
using Entities.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers {
	public class GameManager : MonoBehaviour {

		#region Fields & Properties

		public const string PlayerTag = "Player";
		public const string WeaponTag = "Weapon";

		[SerializeField] private Player player;
		[SerializeField] private CameraController cameraController;


		/// TODO: Eventually, we will revisit this to contemplate a better approach for collecting/spawning entities.
		/// Because ideally, the entities should not even be spawned.
		[Header("Enemy Entities")]
		[SerializeField] private DagonStatue dagonStatue;
		
		public Player Player => player;
		public bool HasFocusOn { get; private set; }
		
		#endregion

		#region Unity Functions

		private void Awake() {
			dagonStatue.Initialize(this);
		}

		private void Start() {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		#endregion

		#region Public Functions

		public void FocusOn(Vector3 target, float duration, Ease ease, TweenCallback callback = null) {
			if (HasFocusOn) return;
			
			HasFocusOn = true;
			player.Aim.FocusOn(target, duration, ease, () => {
				HasFocusOn = false;
				callback?.Invoke();
			});
		}

		#endregion

		#region Private Functions

		#endregion
		
	}
}
