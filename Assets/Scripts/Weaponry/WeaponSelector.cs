using System;
using System.Collections.Generic;
using UnityEngine;

namespace Survival.Weaponry
{
    internal class WeaponSelector : MonoBehaviour
    {
        [SerializeField] float timeBetweenScrolls = 0.2f;

        public event Action<AmmoType> OnWeaponChanged;

        private List<Weapon> weapons = default;
        private int currentWeaponIndex = 0;
        private float scrollTimer = Mathf.Infinity;

        private void Awake()
        {
            weapons = new List<Weapon>(GetComponentsInChildren<Weapon>());
        }

        void Start()
        {
            SetWeaponActive();
        }

        void Update()
        {
            scrollTimer += Time.deltaTime;

            int previousWeapon = currentWeaponIndex;
            ProcessKeyInput();
            ProcessScrollWheel();
            if (previousWeapon != currentWeaponIndex)
            {
                SetWeaponActive();
            }
        }

        private void ProcessScrollWheel()
        {
            if (scrollTimer < timeBetweenScrolls) return;

            var scrollValue = Input.GetAxis("Mouse ScrollWheel");
            if (scrollValue != 0)
            {
                scrollTimer = 0f;
                currentWeaponIndex = (scrollValue > 0) ? --currentWeaponIndex : ++currentWeaponIndex;
                currentWeaponIndex = (weapons.Count + currentWeaponIndex) % weapons.Count;
            }
        }

        private void ProcessKeyInput()
        {
            for (int i = 0, alpha1Value = 49; i < weapons.Count; i++)
            {
                if (Input.GetKeyDown((KeyCode)(alpha1Value + i)))
                {
                    currentWeaponIndex = i;
                    break;
                }
            }
        }

        private void SetWeaponActive()
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].gameObject.SetActive(currentWeaponIndex == i);
            }
            OnWeaponChanged(weapons[currentWeaponIndex].AmmunitionType);
        }
    }
}