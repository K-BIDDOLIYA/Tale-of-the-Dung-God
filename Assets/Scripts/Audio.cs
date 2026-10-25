using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio Instance { get; private set; }

    [Header("Background Music")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip bgmClip;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private AudioClip damageSFX;
    [SerializeField] private AudioClip collectSFX;
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioClip deathSFX;

    [Header("Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float bgmVolume = 0.5f;

    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;

    private bool hasWon;
    private bool hasDied;

    private void Awake()
    {
        // Keep only one Audio manager.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Configure the BGM source.
        if (bgmSource != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            bgmSource.volume = bgmVolume;

            if (bgmClip != null)
                bgmSource.Play();
        }

        // Configure the SFX source.
        if (sfxSource != null)
        {
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
            sfxSource.volume = sfxVolume;
        }
    }

    private void Update()
    {
        // Keep volume changes reflected while playing.
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // Button clicks: menus, pause, restart, etc.
    public void PlayClick()
    {
        PlaySFX(clickSFX);
    }

    // When the player successfully hits a guard.
    public void PlayHit()
    {
        PlaySFX(hitSFX);
    }

    // When the player receives damage.
    public void PlayDamage()
    {
        PlaySFX(damageSFX);
    }

    // When an emerald or other collectible is picked up.
    public void PlayCollect()
    {
        PlaySFX(collectSFX);
    }

    // When the player wins.
    public void PlayWin()
    {
        if (hasWon || hasDied)
            return;

        hasWon = true;

        if (bgmSource != null)
            bgmSource.Stop();

        PlaySFX(winSFX);
    }

    // When the player dies.
    public void PlayDeath()
    {
        if (hasDied)
            return;

        hasDied = true;

        if (bgmSource != null)
            bgmSource.Stop();

        PlaySFX(deathSFX);
    }

    // Optional controls for music.
    public void PauseBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        if (bgmSource != null && bgmSource.clip != null)
            bgmSource.UnPause();
    }

    public void StopBGM()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);

        if (bgmSource != null)
            bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }
}

