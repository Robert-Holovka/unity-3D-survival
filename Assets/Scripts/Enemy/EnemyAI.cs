using Survival.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Survival.Enemy
{
    [SelectionBase]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRange = 20f;
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] float damage = 25;
        public bool IsProvoked { get; set; }

        // Cached components
        private NavMeshAgent navMeshAgent = default;
        private EnemyHealth health = default;
        private PlayerHealth target = default;
        private Animator animator = default;
        private EnemyGroup enemyGroup = default;

        private const float stoppingOffset = 0.2f;
        private bool isAttacking = false;

        private void Awake()
        {
            health = GetComponent<EnemyHealth>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            target = FindObjectOfType<PlayerHealth>();
            enemyGroup = GetComponentInParent<EnemyGroup>();
        }

        private void OnEnable()
        {
            health.OnDamageTaken += OnDamageTaken;
            health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            health.OnDamageTaken -= OnDamageTaken;
            health.OnDeath -= OnDeath;
        }

        private void OnDamageTaken() => OnProvoked();

        private void OnProvoked()
        {
            IsProvoked = true;
            if (enemyGroup != null)
            {
                enemyGroup.OnChildProvoked();
            }
        }

        private void OnDeath()
        {
            health.enabled = false;
        }

        void Update()
        {
            if (health.IsDead) return;

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (IsProvoked)
            {
                EngageTarget(distanceToTarget);
            }
            else if (distanceToTarget <= chaseRange)
            {
                OnProvoked();
            }
        }

        private void EngageTarget(float distanceToTarget)
        {
            FaceTarget();
            if (target.IsDead)
            {
                navMeshAgent.enabled = false;
                animator.SetBool("Attack", false);
                animator.SetBool("Move", false);
                return;
            }

            if (distanceToTarget >= (navMeshAgent.stoppingDistance + stoppingOffset))
            {
                animator.SetBool("Move", true);
                if (!isAttacking)
                {
                    navMeshAgent.SetDestination(target.transform.position);
                }
            }
            else if (!isAttacking)
            {
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
        }

        // Animation Event!
        public void OnEnemyHit()
        {
            isAttacking = false;
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget < 2 * navMeshAgent.stoppingDistance)
            {
                target.TakeDamage(damage);
            }
        }

        private void FaceTarget()
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}