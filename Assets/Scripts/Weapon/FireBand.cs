using UnityEngine;

public class FireBand : BaseWeapon
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float destroyAfterSeconds = 15f;

    private float lastFired;

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
            SpawnFireballs();
            lastBeatTime = Time.time;
        }

        lastEnergy = currentEnergy;
    }


    private void SpawnFireballs()
    {
        int fireballCount = 4;
        float angleStep = 360f / fireballCount;
        Vector3 startPosition = transform.position;

        for (int i = 0; i < fireballCount; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * Vector3.forward;

            GameObject fireball = Instantiate(fireballPrefab, startPosition, Quaternion.identity);
            fireball.GetComponent<Fireball>().Init(direction, damage, projectileSpeed, destroyAfterSeconds);

        }

    }
}
