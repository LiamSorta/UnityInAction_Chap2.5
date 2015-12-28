using UnityEngine;
using System.Collections;

public class RelativeMovement : MonoBehaviour {
	[SerializeField] private Transform target;								// This script needs a reference to the object to move relative to

	void Update () {
		Vector3 movement = Vector3.zero;									// Start with vector (0,0,0) and add movement components progressively.

		float horInput = Input.GetAxis ("Horizontal");
		float VertInput = Input.GetAxis ("Vertical");
		if (horInput != 0 || VertInput != 0) {								// Only handle movement while arrow keys are pressed
			movement.x = horInput;
			movement.z = VertInput;

			Quaternion tmp = target.rotation;								// Keep the initial rotation to restore after finishing with the target object
			target.eulerAngles = new Vector3 (0, target.eulerAngles.y, 0);
			movement = target.TransformDirection (movement);				// Transform movement direction from Local to Global coorindates
			target.rotation = tmp;

			transform.rotation = Quaternion.LookRotation (movement);		// `LookRotation()` calculates a quaternion facing in that direction
		}
	}
}
