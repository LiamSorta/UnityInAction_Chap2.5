using UnityEngine;
using System.Collections;

public class DeviceOperator : MonoBehaviour {
	public float radius = 1.5f;																	// How far away from the player to activate devices

	void Update() {
		if (Input.GetButtonDown ("Fire3")) {													// Respond to the input button defined in Unity's input settings.
			Collider[] hitColliders = Physics.OverlapSphere (transform.position, radius);		// OverlapSphere() returns a list of nearby objects
			foreach (Collider hitCollider in hitColliders) {
				Vector3 direction = hitCollider.transform.position - transform.position;
				if (Vector3.Dot (transform.forward, direction) > .5f) {							// Only send the message when facing the right direction
					hitCollider.SendMessage ("Operate", SendMessageOptions.DontRequireReceiver);// SendMessage tries to call the named function, regardless of the target's type.
				}
			}
		}
	}
}
