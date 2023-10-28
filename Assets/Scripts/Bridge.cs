using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public GameObject bridge1;
    public GameObject bridge2;
    PersistenceManager pm;
    // Start is called before the first frame update
    void Start()
    {
        pm = PersistenceManager.Instance;
        bridge1.SetActive(false);
        bridge2.SetActive(false);

    }

    // Update is called once per frame 
    void Update()
    {
        if (pm.LeverCounter == 1)
        {
            
            bridge1.SetActive(true);
           
        }
        if (pm.LeverCounter == 2)
        {
            bridge1.SetActive(true);
            bridge2.SetActive(true);
        }
    }
}
