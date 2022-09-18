using System.Security.Cryptography;
using Entities.Player.Holdables;
using UnityEngine;

namespace Entities.Player {
    public class PlayerTool : IHoldable {
        private Player _player;
        
        public PlayerTool(Player player) {
            _player = player;
        }

        public void Clear() {
            MonoBehaviour.Destroy(_player.ToolHolder.transform.GetChild(0).gameObject);
        }
    }
}