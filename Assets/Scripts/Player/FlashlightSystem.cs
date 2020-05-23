using UnityEngine;

namespace Survival.Player
{
    internal class FlashlightSystem : MonoBehaviour
    {
        [SerializeField] float intensityDecay = .1f;
        [SerializeField] float angleDecay = 1f;
        [SerializeField] float minAngle = 40f;

        private Light myLight;

        private void Start()
        {
            myLight = GetComponentInChildren<Light>();
        }

        void Update()
        {
            DecreaseLightAngle();
            DecreaseLightIntensity();
        }

        private void DecreaseLightAngle()
        {
            if (myLight.spotAngle > minAngle)
            {
                myLight.spotAngle -= angleDecay * Time.deltaTime;
            }
        }

        private void DecreaseLightIntensity() => myLight.intensity -= intensityDecay * Time.deltaTime;

        public void RestoreLightAngle(float restoreAngle) => myLight.spotAngle = restoreAngle;

        public void RestoreLightIntensity(float intensityAmount) => myLight.intensity += intensityAmount;
    }
}