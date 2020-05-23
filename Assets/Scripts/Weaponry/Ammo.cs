using TMPro;
using UnityEngine;

namespace Survival.Weaponry
{
    internal enum AmmoType
    {
        Bullet,
        Shell
    }

    internal class Ammo : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ammoAmountTMP;
        [SerializeField] AmmoSlot[] ammoSlots;

        [System.Serializable]
        private class AmmoSlot
        {
            public AmmoType ammoType;
            public int ammoAmount;
        }

        private AmmoType currentAmmoType = default;

        private void OnEnable()
        {
            GetComponent<WeaponSelector>().OnWeaponChanged += UpdateAmmoUI;
        }

        private void OnDisable()
        {
            GetComponent<WeaponSelector>().OnWeaponChanged += UpdateAmmoUI;
        }

        internal int GetCurrentAmmo(AmmoType ammoType)
        {
            AmmoSlot ammoSlot = GetAmmoSlot(ammoType);
            return ammoSlot.ammoAmount;
        }

        internal void ReduceAmmo(AmmoType ammoType)
        {
            AmmoSlot ammoSlot = GetAmmoSlot(ammoType);
            ammoSlot.ammoAmount--;
            UpdateAmmoUI(ammoType);
        }

        internal void IncreaseAmmo(AmmoType ammoType, int ammoAmount)
        {
            AmmoSlot ammoSlot = GetAmmoSlot(ammoType);
            ammoSlot.ammoAmount += ammoAmount;
            UpdateAmmoUI(currentAmmoType);
        }

        private AmmoSlot GetAmmoSlot(AmmoType ammoType)
        {
            foreach (AmmoSlot ammoSlot in ammoSlots)
            {
                if (ammoSlot.ammoType == ammoType)
                {
                    return ammoSlot;
                }
            }
            return null;
        }

        private void UpdateAmmoUI(AmmoType ammoType)
        {
            currentAmmoType = ammoType;
            ammoAmountTMP.text = GetCurrentAmmo(ammoType).ToString();
        }
    }
}