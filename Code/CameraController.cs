using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Simple camera controller movement

    [SerializeField] private float sensitivity;
    [SerializeField] private float movementSpeed;

    private void HandleMouseLook() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y") * -1f; // Inverted for some reason

        Vector3 lookVector = new Vector3(mouseY, mouseX, 0f);
        lookVector *= sensitivity;
        lookVector *= Time.deltaTime;

        transform.Rotate(lookVector);
    }

    private void HandleMovement() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.forward * z + transform.right * x;
        transform.position += moveDir * movementSpeed * Time.deltaTime;
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        HandleMouseLook();
        HandleMovement();
    }
}
