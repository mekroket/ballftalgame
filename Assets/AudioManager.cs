using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource; // Müzik için AudioSource
    public float musicVolume = 0.5f; // Varsayılan ses seviyesi

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişimlerinde yok olmasın
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // AudioSource yoksa ekle
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true; // Müzik sürekli çalsın
            musicSource.volume = musicVolume;
            musicSource.playOnAwake = true;
        }
    }

    // Müzik sesini ayarla (0.0f ile 1.0f arası)
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    // Müziği başlat
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.volume = musicVolume;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Müziği duraklat
    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }

    // Müziği devam ettir
    public void ResumeMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }

    // Müziği durdur
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    // Debug için müzik durumunu kontrol et
    void Update()
    {
        if (musicSource != null && musicSource.clip != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
} 