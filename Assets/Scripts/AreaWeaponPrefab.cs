using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class AreaWeaponPrefab : MonoBehaviour
{
    public float damageInterval = 2f;   // Time between damage ticks
    public float radius = 2f;           // Damage radius
    public int damage = 10;             // Damage per tick
    public LayerMask enemyLayer;        // Layer for enemies
    public GarlicData weapon;
    private SphereCollider sphereCollider;
    public static AreaWeaponPrefab instance;
    private void Start()
    {
        instance = this;    
        sphereCollider = GetComponent<SphereCollider>();
        StartCoroutine(DamagePulseLoop());
    }


    private IEnumerator DamagePulseLoop()
    {
        while (true)
        {
            DealAoEDamage();
            yield return new WaitForSeconds(weapon.stats[weapon.WeaponLevel].cooldown);
        }
    }

    private void DealAoEDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(weapon.stats[weapon.WeaponLevel].damage);
                    Debug.Log("Giving Garlic Damage to Enemies" + weapon.stats[weapon.WeaponLevel].damage);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    public void IncreaseRangeGarlic()
    {
        //this.gameObject.transform.localScale = new Vector3(AreaWeapon.Instance.stats[AreaWeapon.Instance.WeaponLevel].range, AreaWeapon.Instance.stats[AreaWeapon.Instance.WeaponLevel].range, AreaWeapon.Instance.stats[AreaWeapon.Instance.WeaponLevel].range);
        sphereCollider.radius = weapon.stats[weapon.WeaponLevel].range;
    }
}
