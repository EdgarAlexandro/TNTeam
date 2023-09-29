using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSFXManager : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    // Update is called once per frame
   public void ChangeVolume()
    {
        AudioListener.volume = musicSlider.value;
        AudioListener.volume = sfxSlider.value;
        Save();
    }

    private void Load()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }
    private void Save()
    {
        PlayerPrefs.GetFloat("musicVolume",musicSlider.value);
        PlayerPrefs.GetFloat("sfxVolume", sfxSlider.value);
    }
}
