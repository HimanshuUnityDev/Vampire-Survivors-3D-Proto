using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponInventory : MonoBehaviour
{
    public static PlayerWeaponInventory instance;

    public Transform weaponParent; // Assign player or weapon holder
    private List<WeaponData> ownedWeapons = new List<WeaponData>();

    private void Awake()
    {
        instance = this;
    }

    public void AddNewWeapon(WeaponData data)
    {
        if (ownedWeapons.Contains(data))
        {
            Debug.Log("Already owned: " + data.name);
            return;
        }

        ownedWeapons.Add(data);
        GameObject newWeapon = Instantiate(data.BaseWeaponPrefab.gameObject, weaponParent);
        BaseWeapon baseWeapon = newWeapon.GetComponent<BaseWeapon>();
        baseWeapon.SetWeaponData(data);

        baseWeapon.SetWeaponData(data);
    }

    public bool HasWeapon(WeaponData data)
    {
        return ownedWeapons.Contains(data);
    }

     
}
