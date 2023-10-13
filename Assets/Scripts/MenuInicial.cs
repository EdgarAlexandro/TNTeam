using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource buttonClickSound;

    private void Start()
    {
        Load();
    }

    private void Update()
    {
        // Verificar si se ha hecho clic en cualquier parte de la pantalla
        if (Input.GetMouseButtonDown(0))
        {
            // Reproducir el sonido del clic
            PlayClickSound();
        }
    }

    public void ChangeVolume()
    {
        // Ajustar el volumen de la m?sica de fondo seg?n el musicSlider
        backgroundMusic.volume = musicSlider.value;

        // Ajustar el volumen del sonido del bot?n seg?n el sfxSlider
        buttonClickSound.volume = sfxSlider.value;

        Save();
    }

    private void Load()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);

        // Aseg?rate de que los valores iniciales de los AudioSource coincidan con los valores de los deslizadores.
        backgroundMusic.volume = musicSlider.value;
        buttonClickSound.volume = sfxSlider.value;
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }

    public void Exit()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }

    public void PlayClickSound()
    {
        // Reproducir el sonido del clic
        buttonClickSound.Play();
    }
}
