using Survival.Enemy;
using System.Collections;
using UnityEngine;

namespace Survival.Weaponry
{
    internal class Weapon : MonoBehaviour
    {
        [Header("Weapon Configs")]
        [SerializeField] AmmoType ammunitionType;
        internal AmmoType AmmunitionType
        {
            get => ammunitionType;
        }
        [SerializeField] int range = 100;
        [SerializeField] int damage = 50;
        [SerializeField] float timeBetweenShots;

        [Header("VFX")]
        [SerializeField] GameObject crossHair;
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] GameObject hitEffect;

        private bool canShoot;
        private Ammo ammo;
        private Camera FPS_Camera;

        private void Awake()
        {
            ammo = GetComponentInParent<Ammo>();
            FPS_Camera = GetComponentInParent<Camera>();
        }

        private void OnEnable()
        {
            crossHair.SetActive(true);
            canShoot = true;
        }

        private void OnDisable()
        {
            crossHair.SetActive(false);
        }

        void Update()
        {
            if (Input.GetMouseButton(0) && canShoot)
            {
                StartCoroutine(Shoot());
            }
        }

        private IEnumerator Shoot()
        {
            canShoot = false;
            if (ammo.GetCurrentAmmo(ammunitionType) > 0)
            {
                muzzleFlash.Play();
                ProcessRaycast();
                ammo.ReduceAmmo(ammunitionType);
            }
            yield return new WaitForSeconds(timeBetweenShots);
            canShoot = true;
        }

        private void ProcessRaycast()
        {
            bool isHit = Physics.Raycast(
                FPS_Camera.transform.position,
                FPS_Camera.transform.forward,
                out RaycastHit hitInfo,
                range
            );

            if (isHit)
            {
                CreateHitImpact(hitInfo);
                EnemyHealth enemyTarget = hitInfo.transform.GetComponent<EnemyHealth>();

                if (enemyTarget != null)
                {
                    enemyTarget.TakeDamage(damage);
                }
            }
        }

        private void CreateHitImpact(RaycastHit hitInfo)
        {
            GameObject hitImpact = Instantiate(hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(hitImpact, .1f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}