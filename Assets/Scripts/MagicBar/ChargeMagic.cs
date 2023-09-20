using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeMagic : MonoBehaviour
{
    public int maxMagic;
    public int currentMagic;

    public MagicBar magicBar;
    // Start is called before the first frame update
    void Start()
    {
        magicBar.SetMaxMagic(maxMagic);
        magicBar.UpdateText(currentMagic);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Orbe"))
        {
            chargeMagicValue(25);
        }
    }

    // Update is called once per frame
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
