using UnityEngine;

/// <summary>
/// Controls the player movement behaviours in a first person character.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
	private float currentSpeed;
	private readonly float walkSpeed = 7.5f;
	private readonly float runSpeed = 10f;

	private readonly float jumpForce = 100;

	private Vector2 movement;

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
		// Generates player to be on top of the ground
		if (Physics.Raycast(new Vector3(50, 100, 50), Vector3.down, out RaycastHit hit)) {
			transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
		}
	}

	// Update is called once per frame
	private void Update() {
		if (Time.timeScale != 0) {
			isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.25f);

			PlayerInput();

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		} else {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
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
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");

		if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
			isJumping = true;
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			isJumping = false;
		}

		isRunning = Input.GetKey(KeyCode.LeftShift);
	}

	/// <summary>
	/// Moves player character in a direction dependent on player input
	/// </summary>
	private void MovePlayer() {
		moveDir = transform.forward * movement.y + transform.right * movement.x;

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
}
