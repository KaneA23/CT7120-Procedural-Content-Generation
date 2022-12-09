using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player movement behaviours in a first person character.
/// </summary>
public class PlayerMovement : MonoBehaviour {
	private float currentSpeed;
	private readonly float walkSpeed = 7.5f;
	private readonly float runSpeed = 10f;

	private readonly float jumpForce = 600;

	private float hMovement;
	private float vMovement;

	private bool isRunning;

	private bool isJumping;
	private bool isGrounded;

	private Rigidbody rb;
	private Vector3 moveDir;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	// Start is called before the first frame update
	private void Start() {
		// Generates player to be on top of the ground instead of spawning in the sky
		if (Physics.Raycast(new Vector3(50, 50, 50), Vector3.down, out RaycastHit hit)) {
			transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
		}
	}

	// Update is called once per frame
	private void Update() {
		if (Time.timeScale != 0) {
			PlayerInput();
		}
	}

	private void FixedUpdate() {
		if (Time.timeScale != 0) {
			MovePlayer();
		}
	}

	/// <summary>
	/// Collects data on what buttons player are pressing
	/// </summary>
	private void PlayerInput() {
		hMovement = Input.GetAxisRaw("Horizontal");
		vMovement = Input.GetAxisRaw("Vertical");

		if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
			isJumping = true;
		}

		isRunning = Input.GetKey(KeyCode.LeftShift);
	}

	/// <summary>
	/// Moves player character in a direction dependent on player input
	/// </summary>
	private void MovePlayer() {
		moveDir = transform.forward * vMovement + transform.right * hMovement;

		// changes movement speed dependent on left shift being pressed
		if (isRunning && isGrounded) {
			currentSpeed = runSpeed;
		} else {
			currentSpeed = walkSpeed;
		}

		rb.velocity = new Vector3(moveDir.x * currentSpeed, rb.velocity.y, moveDir.z * currentSpeed);

		// if player is touching the ground and is pressing space, jump
		if (isJumping && isGrounded) {
			rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
			isGrounded = false;
		}
	}

	/// <summary>
	/// Resets jump when collide with an object
	/// </summary>
	/// <param name="collision">Object player collided</param>
	private void OnCollisionEnter(Collision collision) {
		isGrounded = true;
		isJumping = false;
	}

	/// <summary>
	/// Prevents jumping mid-air
	/// </summary>
	/// <param name="collision">Object player was previously touching</param>
	private void OnCollisionExit(Collision collision) {
		isGrounded = false;
	}
}
