using Survival.Weaponry;
using UnityEngine;

namespace Survival.Pickups
{
    internal class AmmoPickup : MonoBehaviour
    {
        [SerializeField] AmmoType ammoType = AmmoType.Bullet;
        [SerializeField] int ammoAmount = 5;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponentInChildren<Ammo>().IncreaseAmmo(ammoType, ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}