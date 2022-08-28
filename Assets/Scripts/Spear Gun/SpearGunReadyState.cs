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
        _spearGun.Player.Aim.AimOn();
    }

    public void AimOff() {
        _spearGun.Player.Aim.AimOff();
    }
}