using UnityEngine;
using UnityEngine.UI;

namespace Survival.Player
{
    internal class PlayerHealth : MonoBehaviour
    {
        [SerializeField] float startHealth = 100;
        [Tooltip("Health points per second")]
        [SerializeField] float healthRegeneration = 2f;
        [SerializeField] Image healthFillImage = default;

        public bool IsDead { get; set; } = default;

        private float currentHealth = default;

        private void Start()
        {
            currentHealth = startHealth;
            UpdateHealthUI();
        }

        private void Update()
        {
            RegenerateHealth();
            UpdateHealthUI();
        }

        private void RegenerateHealth()
        {
            currentHealth += healthRegeneration * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, startHealth);
        }

        internal void TakeDamage(float damage)
        {
            currentHealth -= damage;
            UpdateHealthUI();
            if (currentHealth <= 0)
            {
                IsDead = true;
                FindObjectOfType<GameSession>().HandleDeath();
            }
        }

        private void UpdateHealthUI()
        {
            float fillAmount = Mathf.Clamp(currentHealth / startHealth, 0f, 1f);
            healthFillImage.fillAmount = fillAmount;
        }
    }
}

