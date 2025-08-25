using GLTF.Schema;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region Variables Declaration

    private NavMeshAgent agent;
    public float attackRange;
    private Animator animator;
    private ThirdPersonController player;
    public bool UpdateNavMesh = true;
    [SerializeField] int damage;
    [SerializeField] GameObject EnemySoulRelease;
    [SerializeField] GameObject HitEffect;
    [SerializeField] GameObject coinPrefab;

    [SerializeField] int experienceToGive;


    [SerializeField] float CurrentHealth;
    [SerializeField] int maxHealth;
    public HealthBar healthBar;

    public float AttackMultiplayer = 1; //Must be multiple of 0.5


    #endregion

    void Start()
    {
        #region Component References

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        player = ThirdPersonController.Instance;
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        #endregion

        // Auto-find 60BPM object at runtime
        GameObject bpmObject = GameObject.Find("60BPM");
        if (bpmObject != null)
        {
            musicSource = bpmObject.GetComponent<AudioSource>();
            if (musicSource == null)
                Debug.LogWarning("⚠️ 60BPM object found but no AudioSource attached.");
        }
        else
        {
            Debug.LogWarning("⚠️ Could not find 60BPM object in scene!");
        }
    }

    void Update()
    {
        #region Enemy moves toward the player and sets the UpdateNavMesh boolean
        // Controlling Navmesh through a bool UpdateNavMesh so that when Enemies are attacking  they will not change their position at the same time
        // Enemies will attack when player come in attackRange

        if (Player_Controller.instance.gameObject.activeSelf)
        {
            if (UpdateNavMesh)
            {
                {
                    float distance = Vector3.Distance(transform.position, player.transform.position);
                    if (distance <= attackRange)
                    {
                        //agent.ResetPath();  // Stop when too close
                        FacePlayer();
                        animator.SetBool("isAttacking", true);
                        animator.SetBool("isIdle", true);

                    }
                    else
                    {
                        agent.SetDestination(player.transform.position);  // Always chase!
                        animator.SetBool("isIdle", false);

                    }
                }
            }
            else
            {
                FacePlayer();
            }
        }
        else
        {
            // Set the Enemy to Idle animation here 
        }

        #endregion


        if (musicSource == null || !musicSource.isPlaying) return;

        // Get spectrum data from audio
        musicSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

        float currentEnergy = 0f;
        foreach (float sample in samples)
            currentEnergy += sample * sample;

        // Detect "beat"
        if (currentEnergy > lastEnergy * sensitivity && Time.time - lastBeatTime > minBeatInterval)
        {
            Debug.Log("🔥 Beat Detected!");
            PlayAttackAnimation();
            lastBeatTime = Time.time;
        }

        lastEnergy = currentEnergy;
    }
    #region Enemy rotates to face the player

    public void EnableNavMesh(int isenable)
    {
        UpdateNavMesh = isenable == 0;
    }

    void FacePlayer()
    {
        Vector3 dir = Player_Controller.instance.transform.position - transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    #endregion

    #region OnCollisionEnter

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Player_Controller.instance.TakeDamage(damage);

        }
    }


    #endregion

    public void TakeDamage(float damage)
    {
        // To Take Damage isImmune should be false
        CurrentHealth -= damage;
        Instantiate(HitEffect, transform.position, Quaternion.identity);

        // Updating Health Bar
        healthBar.SetHealth(CurrentHealth);
        if (CurrentHealth < 1)
        {
            this.gameObject.SetActive(false);
            Quaternion customRotation = Quaternion.Euler(-30.92f, -28.49f, 0f); // X-rotated, facing up
            Instantiate(EnemySoulRelease, new Vector3(transform.position.x, 0.3f, transform.position.z), customRotation);
            Instantiate(coinPrefab, new Vector3(transform.position.x, 0.2f, transform.position.z),Quaternion.identity);
/*            CoinController.instance.ExpereienceForThisEnemy(experienceToGive);
*/        }
    }
    AudioSource musicSource;
    public float sensitivity = 1.5f;   // Higher = fewer beats
    public float minBeatInterval = 0.3f; // Minimum gap between beats

    private float[] samples = new float[1024];
    private float lastEnergy;
    private float lastBeatTime;



    public void PlayAttackAnimation()
    {
        animator.SetFloat("AttackSpeedMultiplayer", AttackMultiplayer);
        Debug.Log(animator.gameObject.name);
        animator.Play("Mutant Swiping", 0);
    }


}
