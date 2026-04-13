using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 1. THE SINGLETON INSTANCE
    // This static variable holds the one and only AudioManager in the game.
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        // 2. SINGLETON LOGIC
        // If there is no instance yet, make THIS the instance and keep it alive.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Tells Unity not to destroy this when loading new scenes!
        }
        else
        {
            // If another AudioManager already exists (like if we returned to the Main Menu), destroy this duplicate!
            Destroy(gameObject);
        }
    }

    // --- MUSIC METHODS ---

    public void PlayMusic(AudioClip backgroundMusic)
    {
        // If the music is already playing, don't restart it
        if (musicSource.clip == backgroundMusic) return;

        musicSource.clip = backgroundMusic;
        musicSource.loop = true; // Ensure BGM loops
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // --- SOUND EFFECT (SFX) METHODS ---

    public void PlaySFX(AudioClip clip)
    {
        // PlayOneShot allows multiple sounds to play at the exact same time without cutting each other off
        sfxSource.PlayOneShot(clip);
    }

    // Optional: Play SFX with random pitch so repetitive sounds (like shooting) sound more natural!
    public void PlaySFXRandomPitch(AudioClip clip)
    {
        sfxSource.pitch = Random.Range(0.85f, 1.15f);
        sfxSource.PlayOneShot(clip);

        // Reset pitch back to normal on a slight delay (so future sounds aren't permanently altered)
        Invoke(nameof(ResetPitch), 0.1f);
    }

    private void ResetPitch()
    {
        sfxSource.pitch = 1f;
    }
}