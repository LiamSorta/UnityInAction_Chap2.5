using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]								
public class RelativeMovement : MonoBehaviour {
	[SerializeField] private Transform target;								// This script needs a reference to the object to move relative to
	public float rotSpeed = 15.0f;
	public float moveSpeed = 6.0f;

	private CharacterController _charController;

	void Start() {
		_charController = GetComponent<CharacterController> ();				// Here's a pattern you've seen in previous chapters, used for getting access to other components.
	}

	void Update () {
		Vector3 movement = Vector3.zero;									// Start with vector (0,0,0) and add movement components progressively.

		float horInput = Input.GetAxis ("Horizontal");
		float VertInput = Input.GetAxis ("Vertical");
		if (horInput != 0 || VertInput != 0) {								// Only handle movement while arrow keys are pressed
			movement.x = horInput * moveSpeed;								// Overwrite the existing X and Z lines to apply movement speed.
			movement.z = VertInput * moveSpeed;
			movement = Vector3.ClampMagnitude (movement, moveSpeed);		// Limit diagonal movement to the same speed as movement along an axis.

			Quaternion tmp = target.rotation;								// Keep the initial rotation to restore after finishing with the target object
			target.eulerAngles = new Vector3 (0, target.eulerAngles.y, 0);
			movement = target.TransformDirection (movement);				// Transform movement direction from Local to Global coorindates
			target.rotation = tmp;

			Quaternion direction = Quaternion.LookRotation (movement);
			transform.rotation = Quaternion.Lerp (transform.rotation, direction, rotSpeed * Time.deltaTime);
			//transform.rotation = Quaternion.LookRotation (movement);		// `LookRotation()` calculates a quaternion facing in that direction
		}

		movement *= Time.deltaTime;											// Remeber to always multiply movement by deltaTime to make them frame rate-independent
		_charController.Move (movement);
	}
}
