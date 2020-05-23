using Survival.Player;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Survival.Enemy
{
    internal class EnemyHealth : MonoBehaviour
    {
        [SerializeField] int health = 40;
        public bool IsDead { get; set; } = false;
        internal event Action OnDamageTaken = default;
        internal event Action OnDeath = default;

        public void TakeDamage(int damage)
        {
            if (IsDead) return;

            health -= damage;
            OnDamageTaken();
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            IsDead = true;
            FindObjectOfType<GameSession>().EnemyKilled();
            GetComponentInChildren<Animator>().SetTrigger("Die");
            GetComponent<NavMeshAgent>().enabled = false;
            OnDeath();
        }
    }
}
