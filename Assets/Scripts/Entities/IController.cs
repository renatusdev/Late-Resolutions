using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController {

    public  Animator            Animator            { get;      } 
    public  CharacterController CharacterController { get;      }
    public  Vector3             Velocity            { get;      }
    public  float               MovementSpeed       { get; set; }

    void Move(Vector3 velocity);
    void OnPause();
    void OnUnpause();
}