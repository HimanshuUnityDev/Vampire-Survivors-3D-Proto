using UnityEngine;

public class SpectrumBeatPrinter : MonoBehaviour
{
    public AudioSource musicSource;
    public float sensitivity = 1.5f; // Higher = less beats
    public float minBeatInterval = 0.3f; // Minimum time (sec) between beats

    private float[] samples = new float[1024];
    private float lastEnergy;
    private float lastBeatTime;

    void Update()
    {
        if (!musicSource.isPlaying) return;

        // Get spectrum data from audio
        musicSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

        float currentEnergy = 0f;
        foreach (float sample in samples)
            currentEnergy += sample * sample;

        // Detect "beat" when energy rises sharply
        if (currentEnergy > lastEnergy * sensitivity && Time.time - lastBeatTime > minBeatInterval)
        {
            Debug.Log("🔥 Beat Detected!");
            lastBeatTime = Time.time; // Save time of this beat
        }

        lastEnergy = currentEnergy;
    }
}
