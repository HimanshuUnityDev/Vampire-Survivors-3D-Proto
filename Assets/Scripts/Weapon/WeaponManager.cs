using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponData> WeaponData;
    public Transform Handposition;

    private void Awake()
    {
        foreach (var weapon in WeaponData)
        {
            weapon.InitializeWeapon();
            if (weapon.name == "Sword")
            {
                var newSword = Instantiate(weapon.BaseWeaponPrefab, Handposition);

                // Set position & rotation relative to the hand
                newSword.transform.localPosition = new Vector3(-0.513000011f, 0.0340000018f, 0.150000006f);
                newSword.transform.localRotation = Quaternion.Euler(7.6f, -74.9f, 94.2f);
                continue;
            }
            else
            {
                Instantiate(weapon.BaseWeaponPrefab, transform.position, Quaternion.identity);

            }
            }
        }

    }
