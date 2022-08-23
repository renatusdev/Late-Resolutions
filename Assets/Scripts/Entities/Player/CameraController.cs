using UnityEngine;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour {

	private CinemachineVirtualCamera m_CinemachineVirtualCamera;
	private Tweener m_FOVInterpolation;
	
	private void Awake() {
		m_CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
		m_FOVInterpolation = DOTween.To(() => m_CinemachineVirtualCamera.m_Lens.FieldOfView,
			x => m_CinemachineVirtualCamera.m_Lens.FieldOfView = x, 0, 0).Pause().SetAutoKill(false);
	}
	
	public void SetFOV(float value, float duration) {
		m_FOVInterpolation.ChangeValues(m_CinemachineVirtualCamera.m_Lens.FieldOfView, value, duration);
		m_FOVInterpolation.Restart();
	}
}