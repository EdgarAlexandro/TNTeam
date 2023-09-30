using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeMagic : MonoBehaviour
{
    public int maxMagic;
    public int currentMagic;
    public MagicBar magicBar;

    void Start()
    {
        magicBar.SetMaxMagic(maxMagic);
        magicBar.UpdateText(currentMagic);
    }

    public void chargeMagicValue(int value)
    {
        if(currentMagic < maxMagic)
        {
            currentMagic += value;
            magicBar.SetMagic(currentMagic);
            magicBar.UpdateText(currentMagic);
        }
    }
}
