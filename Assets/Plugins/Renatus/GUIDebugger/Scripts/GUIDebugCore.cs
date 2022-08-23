#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GUIDebugTool
{

    // TODO(Sergio): isStatic LogBlock that connects via a key.
    public class GUIDebugCore : MonoBehaviour {

        [SerializeField] public Pool m_LogBlocksPool;

        private void Awake() {
            GUIDebug.GUIDebugCore = this;
            m_LogBlocksPool.Initialize();
        }

        // private void Update() {
        //     if(UnityEngine.InputSystem.Keyboard.current.uKey.wasReleasedThisFrame) {
        //         Log("Current time is: " + Time.time);
        //     }
        // }

        public void Log(string text) {        
            LogBlock logBlock = m_LogBlocksPool.Dequeue().GetComponent<LogBlock>();
            logBlock.Initialize(text, m_LogBlocksPool);
        }
    }

    public static class GUIDebug {

        public static GUIDebugCore GUIDebugCore {get; set;}

        public static void Log(string text) {
            GUIDebugCore.Log(text);
        }
    }
}
#endif