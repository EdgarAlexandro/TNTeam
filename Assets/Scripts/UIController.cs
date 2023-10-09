using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{   
    //public AbrirPuertas abrirPuertas;
    public Barradellave keyBar;
    public MagicBar magicBar;
    private PersistenceManager pm;

    void Start()
    {
        pm = PersistenceManager.Instance;
        keyBar.SetMaxKeys(pm.MaxKeys);
        keyBar.SetKeys(pm.CurrentKeys);
        keyBar.UpdateText(pm.CurrentKeys);
        magicBar.SetMaxMagic(pm.MaxMagic);
        magicBar.SetMagic(pm.CurrentMagic);
        magicBar.UpdateText(pm.CurrentMagic);
    }

    public void chargeMagicValue(int value)
    {
        if (pm.CurrentMagic < pm.MaxMagic && pm.CurrentMagic + value < pm.MaxMagic)
        {
            pm.CurrentMagic += value;
        } 
        else
        {
            pm.CurrentMagic = pm.MaxMagic;
        }
        magicBar.SetMagic(pm.CurrentMagic);
        magicBar.UpdateText(pm.CurrentMagic);
    }

    public void loseMagicValue(int value)
    {
        if (pm.CurrentMagic > 0)
        {
            pm.CurrentMagic -= value;
        }
        magicBar.SetMagic(pm.CurrentMagic);
        magicBar.UpdateText(pm.CurrentMagic);

    }

    public void increaseKeyCount(int value)
    {
        if (pm.CurrentKeys < pm.MaxKeys)
        {
            pm.CurrentKeys += value;
            keyBar.SetKeys(pm.CurrentKeys);
            keyBar.UpdateText(pm.CurrentKeys);
        }
       
    }
}
