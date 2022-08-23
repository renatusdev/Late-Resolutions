using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

// TODO(Sergio): .Pop(): Increase the size of the inactive pool.
// TODO(Sergio): Conditional compilation for debug exclusive in-editor?
public class Pool : MonoBehaviour {

    #region Fields

    [SerializeField] private GameObject[] poolObjects;

    private Type t;
    private Queue<GameObject> _queue;
    public GameObject[] PoolObjects => poolObjects;

    #endregion

    #region Public Functions

    /// <summary> Turns off all pool objects, adds this game object as the parent, resets its position and rotation. </summary>
    public void Initialize() {
        _queue = new Queue<GameObject>();
        foreach (var poolObject in poolObjects) {
            Restart(poolObject);
            _queue.Enqueue(poolObject);
        }
    }

    /// <summary> Returns an unused object in the Pool if it exists, else it returns null.
    /// It is expected that the system that uses this GameObject, holds its reference,
    /// so as to return it back to the pool at whichever event the system desires. </summary>
    public GameObject Dequeue() {
        if (_queue.Count <= 0) return null;

        var gO = _queue.Dequeue();
        gO.SetActive(true); 
        return gO;
    }

    /// <summary> Sets a pool object back to the pool, resetting its position, position, and game state. </summary>
    public void Enqueue(GameObject gO) {
        Restart(gO);
        _queue.Enqueue(gO);
    }
    
    #endregion

    #region Private Functions

    private void Restart(GameObject gO) {
        gO.transform.SetParent(transform);
        gO.transform.localPosition = Vector3.zero;
        gO.transform.localRotation = Quaternion.identity;
        gO.SetActive(false);    
    }

    #endregion
}