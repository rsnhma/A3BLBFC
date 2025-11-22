using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("----- Audio Sources ------")]
    [SerializeField] AudioSource musicSource;      // For background music
    [SerializeField] AudioSource ambienceSource;   // For ambience
    [SerializeField] AudioSource SFXSource;        // For clicks, keys, etc

    [Header("----- Audio Clips ------")]
    public AudioClip background;
    public AudioClip ambience;
    public AudioClip click;
    public AudioClip doorOpen;
    public AudioClip key;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        // Start background music
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();

        // Start ambience at the same time (optional)
        ambienceSource.clip = ambience;
        ambienceSource.loop = true;
        ambienceSource.Play();
    }

    public void PlayAmbienceSound()
    {
        // If you only want to trigger ambience later instead of at Start
        ambienceSource.clip = ambience;
        ambienceSource.loop = true;
        ambienceSource.Play();
    }

    public void PlayClick()
    {
        SFXSource.PlayOneShot(click);
    }

    public void PlayDoorOpen()
    {
        SFXSource.PlayOneShot(doorOpen);
    }

    public void PlayKey()
    {
        SFXSource.PlayOneShot(key);
    }
}
