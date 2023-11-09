using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicSFXManager : MonoBehaviour
{
    public static MusicSFXManager Instance { get; private set; }
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioSource levelMusic;
    [SerializeField] AudioSource Click_Button;

    public AudioClip Espada;
    public AudioClip Arco;
    public AudioClip Explosion;
    public AudioClip Baston;
    public AudioClip Mordisco;
    public AudioClip Muerte_Nahual;
    public AudioClip Caja_Rota;
    public AudioClip Campo_Fuerza;
    public AudioClip Daga;
    public AudioClip Lanza_Daga;
    public AudioClip Joker_Sound;
    public AudioClip Carga_Espada;
    public AudioClip Shield;

    private string currentSceneName;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "StartMenu")
        {
            // Reproduce la música del menú
            PlayMenuMusic();
        }
        else
        {
            // Reproduce la música del nivel
            PlayLevelMusic();
        }
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        }

        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 1);
        }

        Load();

    }

    private void Update()
    {
        if (currentSceneName == "StartMenu")
        {
            // Verificar si se ha hecho clic en cualquier parte de la pantalla
            if (Input.GetMouseButtonDown(0))
            {
                // Reproducir el sonido del clic solo en la escena "StartMenu"
                PlayClickSound();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Detecta cuando se carga una nueva escena
        currentSceneName = scene.name;
        if (currentSceneName == "StartMenu")
        {
            // Reproduce la música del menú
            PlayMenuMusic();
        }
        else
        {
            // Reproduce la música del nivel
            PlayLevelMusic();
        }
    }

    public void PlayClickSound()
    {
        // Reproducir el sonido del clic solo en la escena "StartMenu"
        Click_Button.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        GameObject soundObject = new GameObject("SFXAudio");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.volume = PlayerPrefs.GetFloat("sfxVolume", 1.0f);
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(soundObject, clip.length);
    }

    public void PlayMenuMusic()
    {
        menuMusic.Play();
        levelMusic.Stop();
    }

    public void PlayLevelMusic()
    {
        menuMusic.Stop();
        levelMusic.Play();
    }


    public void ChangeVolume()
    {
        menuMusic.volume = musicSlider.value;
        levelMusic.volume = musicSlider.value;
        Click_Button.volume = sfxSlider.value;
        Save();
    }

    private void Load()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }
}
