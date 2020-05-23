using UnityEngine;
using UnityEngine.UI;

namespace Survival.Player
{
    internal class Movement : MonoBehaviour
    {
        [SerializeField] float walkingSpeed = 5f;
        [SerializeField] float runningSpeed = 10f;
        [SerializeField] float jumpForce = 5f;

        [Header("Stamina")]
        [SerializeField] float startStamina = 100f;
        [SerializeField] float staminaDecayRate = 20f;
        [SerializeField] float staminaRegenerationRate = 5f;
        [SerializeField] Image staminaFillImage = default;

        // Cached Components
        private Rigidbody rigidBody = default;
        private Animator staminaAnimator = default;

        // Movement
        private float currentSpeed = default;
        private Vector3 velocity = Vector3.zero;
        // Jumping
        private bool canJump = true;
        private bool isJumping = false;
        // Stamina
        private float currentStamina = default;
        private bool isExhausted = false;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            staminaAnimator = staminaFillImage.GetComponentInParent<Animator>();
        }

        private void Start()
        {
            currentSpeed = walkingSpeed;
            currentStamina = startStamina;
            staminaAnimator.enabled = false;
        }

        private void Update()
        {
            isJumping = Input.GetKeyDown(KeyCode.Space);
            CalculateSpeed();
            CalculateVelocity();
            UpdateStaminaUI();
        }

        private void FixedUpdate()
        {
            Move();
            Jump();
            UpdateStamina();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                canJump = true;
            }
        }

        private void CalculateSpeed()
        {
            if (isExhausted)
            {
                currentSpeed = walkingSpeed;
                return;
            }
            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runningSpeed : walkingSpeed;
        }

        private void CalculateVelocity()
        {
            Vector3 verticalInput = transform.forward * Input.GetAxisRaw("Vertical");
            Vector3 horizontalInput = transform.right * Input.GetAxisRaw("Horizontal");
            velocity = (verticalInput + horizontalInput).normalized * currentSpeed;
        }

        private void Move()
        {
            if (velocity == Vector3.zero) return;

            Vector3 offset = velocity * Time.fixedDeltaTime;
            Vector3 newPosition = rigidBody.position + offset;
            rigidBody.MovePosition(newPosition);
        }

        private void Jump()
        {
            if (canJump && isJumping)
            {
                canJump = false;
                isJumping = false;
                rigidBody.AddRelativeForce(Vector3.up * jumpForce);
            }
        }

        private void UpdateStamina()
        {
            if (velocity != Vector3.zero && currentSpeed == runningSpeed)
            {
                currentStamina -= staminaDecayRate * Time.fixedDeltaTime;
            }
            else
            {
                currentStamina += staminaRegenerationRate * Time.fixedDeltaTime;
            }

            currentStamina = Mathf.Clamp(currentStamina, 0f, startStamina);
        }

        private void UpdateStaminaUI()
        {
            if (currentStamina == 0)
            {
                isExhausted = true;
                staminaAnimator.enabled = true;
            }
            if (currentStamina == startStamina)
            {
                isExhausted = false;
                staminaAnimator.enabled = false;
            }

            staminaFillImage.fillAmount = currentStamina / startStamina;
        }
    }
}