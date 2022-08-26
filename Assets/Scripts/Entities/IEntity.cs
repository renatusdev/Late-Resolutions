using UnityEngine;

// TODO: This interface might not be necessary.
namespace Entities
{
    public interface IEntity {

        public  Animator            Animator            { get;      } 
        public  CharacterController CharacterController { get;      }
        public  Vector3             Velocity            { get;      }
        public  float               MovementSpeed       { get; set; }

        void Move(Vector3 velocity);
        void OnPause();
        void OnUnpause();
    }
}