using UnityEngine;
using System.Collections.Generic;
using Autodesk.Fbx;

public class WeaponInventoryManager : MonoBehaviour
{
    public static WeaponInventoryManager instance;

    [Header("Weapon Data")]
    public List<WeaponData> allWeapons; // All weapons in game
    public Transform handPosition;      // Hand-held weapon position
    public Transform playerTransform;   // For non-hand weapons

    [Header("Tracking")]
    public List<WeaponData> ownedWeapons = new List<WeaponData>();
    public List<WeaponData> lockedWeapons = new List<WeaponData>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        lockedWeapons = new List<WeaponData>(allWeapons); // Start all locked
    }

    private void Start()
    {
        // Give starting weapon (first in list or specific one)
        if (lockedWeapons.Count > 0)
        {
            UnlockWeapon(lockedWeapons[0]);
        }
    }

    public void UnlockWeapon(WeaponData weaponData)
    {

        if (ownedWeapons.Contains(weaponData)) return;

        ownedWeapons.Add(weaponData);
        weaponData.WeaponLevel++;

        lockedWeapons.Remove(weaponData);

        SpawnWeapon(weaponData);
    }

    public void UpgradeWeapon(WeaponData weaponData)
    {
        // Find the weapon's current instance and upgrade it

        BaseWeapon[] weaponInstances = FindObjectsOfType<BaseWeapon>();
        weaponData.WeaponLevel++;
        foreach (var weapon in weaponInstances)
        {
            if (weapon.weaponData == weaponData)
            {
                weapon.UpgradeToLevel(weaponData.WeaponLevel);
            }
        }
    }

    private void SpawnWeapon(WeaponData weaponData)
    {
        Transform parent = weaponData.isChild ?
                           (weaponData.name == "Sword" ? handPosition : playerTransform)
                           : null;

        BaseWeapon newWeapon;

        if (parent != null)
        {
            newWeapon = Instantiate(weaponData.BaseWeaponPrefab, parent);
            newWeapon.transform.localPosition = new Vector3(-0.5516f, 0.0386f, 0.0881f);
            newWeapon.transform.localRotation = Quaternion.Euler(7.741f, -83.658f, 87.231f);
        }
        else
        {
            newWeapon = Instantiate(weaponData.BaseWeaponPrefab, playerTransform.position, Quaternion.identity);
        }

        // This runs for both child and non-child cases
        if (weaponData.name == "FireBand")
        {
            Vector3 pos = newWeapon.transform.localPosition;

            pos.x = 0.039f;     // Set local X position
            pos.y = 0.88f;  // Set local Y position

            newWeapon.transform.localPosition = pos;
        }
        else
        {
            Debug.Log("Not found");
        }

        BaseWeapon baseWpn = newWeapon.GetComponent<BaseWeapon>();
        baseWpn.SetWeaponData(weaponData);
    }

}
