using UnityEngine;

namespace Survival.Player
{
    public class LookRotation : MonoBehaviour
    {
        [SerializeField] float mouseSensitivity = 5f;
        [SerializeField] float minCameraLookDown = -50f;
        [SerializeField] float maxCameraLookUp = 45f;

        // Cached Components
        private Camera mainCamera = default;
        private Rigidbody rigidBody = default;

        private Quaternion playerRotation = Quaternion.identity;
        private float currentCameraRotation = default;

        private void Awake()
        {
            mainCamera = GetComponentInChildren<Camera>();
            rigidBody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            currentCameraRotation = mainCamera.transform.localRotation.x;
        }

        private void Update()
        {
            float yRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
            playerRotation = Quaternion.Euler(new Vector3(0f, yRotation, 0f));

            float xRotation = Input.GetAxis("Mouse Y") * mouseSensitivity;
            currentCameraRotation -= xRotation;
            currentCameraRotation = Mathf.Clamp(currentCameraRotation, minCameraLookDown, maxCameraLookUp);
        }

        private void FixedUpdate()
        {
            rigidBody.MoveRotation(rigidBody.rotation * playerRotation);
            RotateCamera();
        }

        private void RotateCamera()
        {
            Quaternion cameraRotation = Quaternion.Euler(new Vector3(currentCameraRotation, 0f, 0f));
            mainCamera.transform.localRotation = cameraRotation;
        }
    }
}