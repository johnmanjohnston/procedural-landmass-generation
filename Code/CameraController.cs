using UnityEngine;

namespace TerrainGeneration.MovementController {
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float sensitivity;
        [SerializeField] private float movementSpeed;

        private void HandleMouseLook() {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y") * -1f; // Inverted for some reason

            Vector3 lookVector = new Vector3(mouseY, mouseX, 0f) * sensitivity * Time.deltaTime;
            transform.eulerAngles += lookVector;
        }

        private void HandleMovement() {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;
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
}