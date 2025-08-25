using UnityEngine;

public class Garlic : BaseWeapon
{
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
            DamageEnemiesInRange();
            lastBeatTime = Time.time;
        }

        lastEnergy = currentEnergy;
    }


    private void DamageEnemiesInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Enemy>()?.TakeDamage(damage);
            }
        }
    }
}
