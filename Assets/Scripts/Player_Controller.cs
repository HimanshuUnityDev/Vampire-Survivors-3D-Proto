using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections.Generic;

public class Player_Controller : MonoBehaviour
{
    #region Declaring components


    [SerializeField] float CurrentHealth;
    [SerializeField] int maxHealth;
    public static Player_Controller instance;
    public HealthBar healthBar;
    [SerializeField] private Animator animator;
    public LayerMask enemyLayer;

    public float experience;
    public int currentLevel;
    public int maxLevel;
    public List<int> playerLevels;
    public WeaponData ActiveWeapon;

    public bool isImmune;
    public float isImmuneTimer;

    public List<BaseWeapon> weapons;


    private void Awake()
    {
        instance = this;
    }
    #endregion 
    void Start()
    {
        if(UIController.instance==null)
        {
            Debug.Log("Found null ref");
        }
        UIController.instance.UpdateExperienceSlider();
        #region Intializing components

        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        #endregion
        for (int i = playerLevels.Count; i < maxLevel; i++)
        {
            playerLevels.Add(Mathf.CeilToInt(playerLevels[playerLevels.Count - 1] * 1.1f) + 1);
        }
            

    }
    private void Update()
    {
        /* #region If IsImmuneTimer is less than 0, set the boolean to false; otherwise, gradually decrease the timer value.

         if (isImmuneTimer < 0)
         {
             isImmune = false;
         }
         else
         {
             isImmuneTimer -= Time.deltaTime;
         }
         #endregion*/


        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, 1.5f, enemyLayer);

     /*   if (nearbyEnemies.Length > 0)
        {
            animator.SetBool("IsAttacking", true);
        }
        else
        {
            animator.SetBool("IsAttacking", false);
        }*/

    }

    # region The player takes damage based on the value of the immune boolean
    public void TakeDamage(float damage)
    {
        // To Take Damage isImmune should be false

        CurrentHealth -= damage;

        // Updating Health Bar
        healthBar.SetHealth(CurrentHealth);
        Debug.Log("Player Damaged");
        if (CurrentHealth < 1)
        {
            Debug.Log("Destroying");
            this.gameObject.SetActive(false);
        }
        /* if (!isImmune)
         {
             isImmuneTimer = 1f;
             isImmune = true;
         }*/
    }
    #endregion

   /* private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetBool("IsAttacking", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetBool("IsAttacking", false);

        }
    }*/

    private void DealDamage()
    {
       /* // Find all enemies in front of player within range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, 1.5f); // adjust radius as needed

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>()?.TakeDamage(10);
                Debug.Log("Enemy Damaged");
            }
        }*/
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

    public void GetExperience(float experinceToGet)
    {
        experience += experinceToGet;
        UIController.instance.UpdateExperienceSlider();
        if (experience >= playerLevels[currentLevel - 1])
        {
            LevelUp();
        }
      
    }
    public void LevelUp()
    {
/*        Debug.Log("Expereience before"+experience);
*/        experience -= playerLevels[currentLevel - 1];
/*        Debug.Log("Expereience After"+experience);
*/        currentLevel++;
        UIController.instance.LevelUpPanelOpen();
        UIController.instance.UpdateExperienceSlider();
        UIController.instance.LevelUpButtons[0].ActivateButton();
        WeaponUpgradeManager.instance.ShowRandomWeaponChoices();


    }

}
