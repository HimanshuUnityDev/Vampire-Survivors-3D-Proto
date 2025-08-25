using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [HideInInspector]
    public WeaponData weaponData;
    protected float cooldown = 2f;
    protected float duration = 1f;
    protected float damage = 10f;
    protected float range = 0.7f;
    protected int level = 0;
    public static BaseWeapon instance;

    protected virtual void Start()
    {
        instance = this;
    }
    public void SetWeaponData(WeaponData _weaponData)
    {
        weaponData = _weaponData;
        level = weaponData.WeaponLevel;
        UpgradeWeaponStatus();
    }
    public void UpgradeToLevel(int passedLevel)
    {
        level = passedLevel;
        UpgradeWeaponStatus();
    }

    private void UpgradeWeaponStatus()
    {
        cooldown = weaponData.stats[level].cooldown;
        duration = weaponData.stats[level].duration;
        damage = weaponData.stats[level].damage;
        range = weaponData.stats[level].range;

    }
    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
/*             Debug.Log(weaponData.WeaponType);
*/        }
    }

}
