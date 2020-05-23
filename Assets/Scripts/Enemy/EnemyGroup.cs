using UnityEngine;

namespace Survival.Enemy
{
    internal class EnemyGroup : MonoBehaviour
    {
        private EnemyAI[] children;

        private void Awake()
        {
            children = transform.GetComponentsInChildren<EnemyAI>();
        }

        internal void OnChildProvoked()
        {
            foreach (EnemyAI child in children)
            {
                child.IsProvoked = true;
            }
        }
    }
}

