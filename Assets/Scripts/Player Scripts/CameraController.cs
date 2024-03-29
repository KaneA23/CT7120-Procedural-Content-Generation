using UnityEngine;

/// <summary>
/// Controls the position of the player's first person camera.
/// </summary>
public class CameraController : MonoBehaviour {
	private readonly float mouseSensitivity = 10f;
	private float xRotation = 0f;

	private Vector2 mousePos;
	
	// Turning playerbody keeps forward as direction player is facing
	private Transform playerBody;	

	private void Awake() {
		playerBody = transform.parent;
	}

	// Start is called before the first frame update
	private void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	private void Update() {
		if (Time.timeScale != 0) {
			PlayerInput();
		}
	}

	private void LateUpdate() {
		if (Time.timeScale != 0) {
			RotateCamera();
		}
	}

	/// <summary>
	/// Collects data on where the mouse is within the screen
	/// </summary>
	private void PlayerInput() {
		mousePos.x = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
		mousePos.y = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
	}

	/// <summary>
	/// Updates camera rotation dependent on where player mouses the mouse
	/// </summary>
	private void RotateCamera() {
		xRotation -= mousePos.y;
		xRotation = Mathf.Clamp(xRotation, -60f, 60f);

		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		playerBody.Rotate(Vector3.up * mousePos.x);
	}
}
