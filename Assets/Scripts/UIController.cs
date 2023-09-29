using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public int maxKeys;
    public int currentKeys;
    public Barradellave barradellave;
    public AbrirPuertas abrirPuertas;
    public int maxMagic;
    public int currentMagic;
    public MagicBar magicBar;

    void Start()
    {
        barradellave.SetMaxKeys(maxKeys);
        barradellave.UpdateText(currentKeys);
        magicBar.SetMaxMagic(maxMagic);
        magicBar.UpdateText(currentMagic);
    }
    public void chargeMagicValue(int value)
    {
        if (currentMagic < maxMagic)
        {
            currentMagic += value;
        } 
        else
        {
            currentMagic = maxMagic;
        }
        magicBar.SetMagic(currentMagic);
        magicBar.UpdateText(currentMagic);
    }

    public void increaseKeyCount(int value)
    {
        if (currentKeys < maxKeys)
        {
            currentKeys += value;
            barradellave.SetKeys(currentKeys);
            barradellave.UpdateText(currentKeys);
            abrirPuertas.OpenDoor(currentKeys);
        }
       
    }
}
