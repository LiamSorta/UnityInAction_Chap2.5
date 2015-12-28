using UnityEngine;
using System.Collections;

public class DoorOpenDevice : MonoBehaviour {
	[SerializeField] private Vector3 dPos;			// The position to offset to when the door opens

	private bool _open;								// A Boolean to keep track of the open state of the door

	public void Operate(){
		if (_open) {								// Open or close the door depending on the open state.
			Vector3 pos = transform.position - dPos;
			transform.position = pos;
		} else {
			Vector3 pos = transform.position + dPos;
			transform.position = pos;
		}
		_open = !_open;
	}

	public void Activate() {
		if (!_open) {
			Vector3 pos = transform.position + dPos;
			transform.position = pos;
			_open = true;
		}
	}

	public void Deactivate() {
		if (_open) {
			Vector3 pos = transform.position - dPos;
			transform.position = pos;
			_open = false;
		}
	}
}
