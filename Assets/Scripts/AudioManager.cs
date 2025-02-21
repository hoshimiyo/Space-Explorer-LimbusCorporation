using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip starSound;
    public AudioClip starPickupSound;
    public AudioClip explosionSound;
    public AudioClip laserSound;
    private AudioSource audioSource;
    public AudioSource backgroundMusicSource;
    public AudioClip backgroundMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true; // Loop the music
        backgroundMusicSource.volume = 0.5f; // Set volume level
        backgroundMusicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void ToggleMusic(bool isPlaying)
    {
        if (isPlaying)
            backgroundMusicSource.Play();
        else
            backgroundMusicSource.Pause();
    }
}
