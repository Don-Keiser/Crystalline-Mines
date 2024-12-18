using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Clips")]
    public AudioClip jumpSound;
    public AudioClip runSound;
    public AudioClip deathSound;
    public AudioClip doorSound;
    public AudioClip inGameMusic;
    public AudioClip gameOverSong;
    public AudioClip mainMenuSong;

    private AudioSource _audioSource;
    private AudioSource _musicSource;
    private bool _isRunning;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _audioSource = Settings.Instance.sfxSource;
        _musicSource = Settings.Instance.musicSource;
    }

    private void Start()
    {
        _musicSource.Stop();
        _musicSource.clip = inGameMusic;
        _audioSource.loop = true;
        _musicSource.Play();
    }

    public void PlaySound(AudioClip clip, bool loop = false)
    {
        if (clip is null) 
            Debug.LogError("ERROR ! The given clip is null.");

        if (loop)
        {
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }
        else
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    public void StopSound()
    {
        _audioSource.loop = false;
        _audioSource.Stop();
    }
}
