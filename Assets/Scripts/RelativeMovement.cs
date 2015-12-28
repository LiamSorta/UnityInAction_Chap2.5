using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]								
public class RelativeMovement : MonoBehaviour {
	[SerializeField] private Transform target;								// This script needs a reference to the object to move relative to
    [SerializeField] GameObject energyUI;
	public float rotSpeed = 15.0f;
	public float moveSpeed = 6.0f;
	public float jumpSpeed = 15.0f;
	public float gravity = -9.8f;
	public float terminalVelocity = -10.0f;
	public float minFall = 1.5f;
	public float pushForce = 3.0f;											// Amount of force to apply
    public float exhaustAmount = 0.005f;
    public float regenAmount = 0.04f;

	private CharacterController _charController;
	private ControllerColliderHit _contact;
	private Animator _animator;
	private float _vertSpeed;

	void Start() {
		_vertSpeed = minFall;												// Initialize the vertical speed to the minimum falling speed at the start of the existing function
		_charController = GetComponent<CharacterController> ();				// Here's a pattern you've seen in previous chapters, used for getting access to other components.
		_animator = GetComponent<Animator> ();
	}

	void Update () {
		Vector3 movement = Vector3.zero;									// Start with vector (0,0,0) and add movement components progressively.

        //When moving, deplete energy -- using keys over axis 
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
            energyUI.GetComponent<TiredPlayer>().Exhaust(exhaustAmount);
        else
            energyUI.GetComponent<TiredPlayer>().Exhaust(-regenAmount);

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
		_animator.SetFloat ("Speed", movement.sqrMagnitude);

		bool hitGround = false;
		RaycastHit hit;

		if (_vertSpeed < 0 && 
			Physics.Raycast (transform.position, Vector3.down, out hit)) {	// Check if the player is falling
			float check = (_charController.height + _charController.radius) / 1.9f;	// The distance to check against (extend slightly beyond the bottom of the capsule)
			hitGround = hit.distance <= check;
		}

		if (hitGround) {													// Instead of using `isGrounded`, check the raycasting result.
			if (Input.GetButtonDown ("Jump")) {								// React to the Jump button while on the ground.
				_vertSpeed = jumpSpeed;
			} else {
				_vertSpeed = -0.1f;
				_animator.SetBool ("Jumping", false);
			}
		} else {															// If not on the ground, then apply gravity until terminal velocity is reached.
			_vertSpeed += gravity * 5 * Time.deltaTime;

			if (_vertSpeed < terminalVelocity) {
				_vertSpeed = terminalVelocity;
			}

			if (_contact != null) {
				_animator.SetBool ("Jumping", true);
			}

			if (_charController.isGrounded) {								// Raycasting didn't detect ground, but the capsule is touching the ground
				if (Vector3.Dot (movement, _contact.normal) < 0) {			// Respond slighlty differently depending on whether the character is facing the contact point
					movement = _contact.normal * moveSpeed;
				} else {
					movement += _contact.normal * moveSpeed;
				}
			}
		}
		movement.y = _vertSpeed;

		movement *= Time.deltaTime;											// Remember to always multiply movement by deltaTime to make them frame rate-independent
		_charController.Move (movement);
	}

	void OnControllerColliderHit(ControllerColliderHit hit){				// Store the collision data in the callback when a collision is detected.
		_contact = hit;

		Rigidbody body = hit.collider.attachedRigidbody;					// Check if the collided object has a Rigidbody to receive physics forces.
		if (body != null && !body.isKinematic) {
			body.velocity = hit.moveDirection * pushForce;					// Apply velocity to the physics body.
		}
	}
}
