using UnityEngine;
using Cinemachine;
using DG.Tweening;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour {

	private CinemachineVirtualCamera _cinemachineVirtualCamera;
	private Tweener _fovTween;
	
	private void Awake() {
		_cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
		_fovTween = DOTween.To(() => _cinemachineVirtualCamera.m_Lens.FieldOfView,
			x => _cinemachineVirtualCamera.m_Lens.FieldOfView = x, 0, 0).Pause().SetAutoKill(false);
	}
	
	public void SetFOV(float value, float duration) {
		_fovTween.ChangeValues(_cinemachineVirtualCamera.m_Lens.FieldOfView, value, duration);
		_fovTween.Restart();
	}
}