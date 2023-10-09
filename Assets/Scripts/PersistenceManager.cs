using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    private static PersistenceManager instance;
    public static PersistenceManager Instance { get { return instance; } }

    public int CurrentHealth;
    public int MaxHealth;
    public int MaxMagic;
    public int CurrentMagic;
    public int MaxKeys;
    public int CurrentKeys;

    // Singleton
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
