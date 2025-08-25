using UnityEngine;

public class Sword : BaseWeapon
{
    Animator PlayerAnimator;
    public float AttackSpeedMultiplyer = 1; //Must be multiple of 0.5
    AudioSource musicSource;
    public float sensitivity = 1.5f;   // Higher = fewer beats
    public float minBeatInterval = 0.3f; // Minimum gap between beats

    private float[] samples = new float[1024];
    private float lastEnergy;
    private float lastBeatTime;

    protected override void Start()
    {
        base.Start();

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

    protected override void Update()
    {
        base.Update();
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

    private void Awake()
    {
        PlayerAnimator = GetComponentInParent<Animator>();

    }
    private void OnTriggerEnter(Collider other)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, 1.5f); // adjust radius as needed

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>()?.TakeDamage(damage);
            }

        }

    }
    public void PlayAttackAnimation()
    {
        PlayerAnimator.SetFloat("AttackMultiplier", AttackSpeedMultiplyer);
        Debug.Log(PlayerAnimator.gameObject.name);
        PlayerAnimator.Play("Attacking", 0);
    }
}

