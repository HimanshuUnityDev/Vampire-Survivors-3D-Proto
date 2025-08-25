using UnityEngine;

public class LightningWeapon : BaseWeapon
{
    public GameObject lightningPrefab;
    AudioSource musicSource;
    public float sensitivity = 1.5f;   // Higher = fewer beats
    public float minBeatInterval = 0.3f; // Minimum gap between beats

    private float[] samples = new float[1024];
    private float lastEnergy;
    private float lastBeatTime;

    void Start()
    {
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
            StrikeRandomEnemies();
            lastBeatTime = Time.time;
        }

        lastEnergy = currentEnergy;
    }

    void StrikeRandomEnemies()
    {
        Debug.Log("⚡ Lightning Strike triggered!");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0) return;

        // Pick 1–3 random enemies
        int strikes = Mathf.Min(3, enemies.Length);

        for (int i = 0; i < strikes; i++)
        {
            GameObject enemy = enemies[Random.Range(0, enemies.Length)];

            Vector3 hitPos = enemy.transform.position;

            // Spawn VFX
            Instantiate(lightningPrefab, hitPos, Quaternion.identity);

            // Apply damage
            enemy.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }
}
