using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerCBT : MonoBehaviour
{


    public MusicSFXManager soundEffect;
    // Start is called before the first frame update
    void Start()
    {
        soundEffect = MusicSFXManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SoundEffect(AudioClip clip)
    {
        soundEffect.PlaySFX(clip);
        Debug.Log("Hola");
    }

}
