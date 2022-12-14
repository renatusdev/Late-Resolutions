using System;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.Renatus.Util.Events {
	[RequireComponent(typeof(Collider))]
	public class OnTriggerEventHelper : MonoBehaviour {
		
		private Action<Collider> _onTriggerEnterAction;
		public Collider Collider { get; private set; }

		private void Start() {
			Collider = GetComponent<Collider>();
		}

		public void Initialize(Action<Collider> onTriggerEnter) {
			_onTriggerEnterAction = onTriggerEnter;
		}

		private void OnTriggerEnter(Collider other) {
			_onTriggerEnterAction?.Invoke(other);
		}
	}
}
