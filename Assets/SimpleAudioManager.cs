using UnityEngine;

public class SimpleAudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip backgroundMusic;

    void Start()
    {
        // Audio Source component'ini al
        audioSource = GetComponent<AudioSource>();
        
        // Eğer müzik atanmışsa başlat
        if (backgroundMusic != null && audioSource != null)
        {
            Debug.Log("Müzik ayarlanıyor...");
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.volume = 0.5f;
            audioSource.Play();
            Debug.Log("Müzik başlatıldı!");
        }
        else
        {
            Debug.LogError("Müzik veya AudioSource eksik!");
        }
    }
} 