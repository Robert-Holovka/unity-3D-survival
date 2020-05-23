using Survival.Player;
using UnityEngine;

namespace Survival.Pickups
{
    internal class BatteryPickup : MonoBehaviour
    {
        [SerializeField] float restoreAngle = 90f;
        [SerializeField] float intensityAmount = 1f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                FlashlightSystem flashLight = other.GetComponent<FlashlightSystem>();
                flashLight.RestoreLightAngle(restoreAngle);
                flashLight.RestoreLightIntensity(intensityAmount);
                Destroy(gameObject);
            }
        }
    }
}
