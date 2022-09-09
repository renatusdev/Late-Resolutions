using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Entities.Player {
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class CameraController : MonoBehaviour {

		private CinemachineVirtualCamera _cinemachineVirtualCamera;
		private Tweener _fovTween;
		private Tweener _dutchTween;
		private float _fov;
	
		private void Awake() {
			_cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
			_fov = _cinemachineVirtualCamera.m_Lens.FieldOfView;
			_fovTween = DOTween.To(() => _cinemachineVirtualCamera.m_Lens.FieldOfView,
				x => _cinemachineVirtualCamera.m_Lens.FieldOfView = x, 0, 0).Pause().SetAutoKill(false);
			
			_dutchTween = DOTween.To(() => _cinemachineVirtualCamera.m_Lens.Dutch,
				x => _cinemachineVirtualCamera.m_Lens.Dutch = x, 0, 0).Pause().SetAutoKill(false);
		}
	
		public void SetFOVZoomIn(float duration) {
			_fovTween.ChangeValues(_cinemachineVirtualCamera.m_Lens.FieldOfView, _fov - 15, duration);
			_fovTween.Restart();
		}
		
		public void SetFOVZoomOut(float duration) {
			_fovTween.ChangeValues(_cinemachineVirtualCamera.m_Lens.FieldOfView, _fov, duration);
			_fovTween.Restart();
		}

		public void SetDutch(float value, float duration) {
			_dutchTween.ChangeValues(_cinemachineVirtualCamera.m_Lens.Dutch, value, duration);
			_dutchTween.Restart();
		}
	}
}