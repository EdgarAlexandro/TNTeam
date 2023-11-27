using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    MusicSFXManager soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        soundEffect = MusicSFXManager.Instance;
    }

    public void SoundEffect(AudioClip clip)
    {
        soundEffect.PlaySFX(clip);
    }
}
