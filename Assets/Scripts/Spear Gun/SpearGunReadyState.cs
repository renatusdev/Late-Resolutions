using Plugins.Renatus.Util.State_Machine;
using Spear_Gun;
using UnityEngine.InputSystem;

public class SpearGunReadyState : IBaseState {
    private SpearGun _spearGun;

    public SpearGunReadyState(SpearGun spearGun) {
        _spearGun = spearGun;
    }
    
    public void Execute() {
    }

    public void Enter() {
    }

    public void Exit() {
        AimOff();
    }

    public void AimOn() {
        _spearGun.PlayerController.Animator.SetBool("isAiming", true);
        _spearGun.PlayerController.CameraController.SetFOV(75, 0.4f);
    }

    public void AimOff() {
        _spearGun.PlayerController.Animator.SetBool("isAiming", false);
        _spearGun.PlayerController.CameraController.SetFOV(90, 0.2f);
    }
}