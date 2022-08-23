using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable {

	void OnHit(Spear spear, Vector3 hitPoint);
}
