using UnityEngine;
using System.Collections.Generic;

public class WeaponUpgradeManager : MonoBehaviour
{
    public List<LevelUpButton> levelUpButtons; // UI buttons
    public static WeaponUpgradeManager instance;

    private void Start()
    {
        instance = this;
    }

    public void ShowRandomWeaponChoices()
    {
        List<WeaponData> pool = new List<WeaponData>(WeaponInventoryManager.instance.lockedWeapons);

        if (pool.Count == 0)
        {
            Debug.Log("All weapons unlocked. Show upgrades instead.");
            pool = new List<WeaponData>(WeaponInventoryManager.instance.ownedWeapons);
        }

        Shuffle(pool);

        for (int i = 0; i < levelUpButtons.Count; i++)
        {
            if (i < 2 && i < pool.Count)
            {
                levelUpButtons[i].gameObject.SetActive(true);
                levelUpButtons[i].weapon = pool[i];
                levelUpButtons[i].ActivateButton();
            }
            else
            {
                levelUpButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void Shuffle(List<WeaponData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            var temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }

    public void OnWeaponSelected(WeaponData weapon)
    {
        if (!WeaponInventoryManager.instance.ownedWeapons.Contains(weapon))
        {
            WeaponInventoryManager.instance.UnlockWeapon(weapon);
        }
        else
        {
            WeaponInventoryManager.instance.UpgradeWeapon(weapon);
        }
    }
}
