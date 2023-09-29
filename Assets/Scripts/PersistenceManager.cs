using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    private static PersistenceManager instance;
    public static PersistenceManager Instance { get { return instance; } }

    //dpublic HealthBar healthBar;
    public HealthBar healthBarPrefab;
    public Canvas canvasPrefab;
    private HealthBar healthBar;


    // Add variables for any other elements you want to persist here

    void Start()
    {
        Canvas canvas = Instantiate(canvasPrefab);
        canvas.renderMode = RenderMode.WorldSpace;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.transform.SetParent(canvas.transform);
    }

    

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

    /*
    public void AlterHealthBar()
    {
        //HealthBar healthBar = Instantiate(healthBarPrefab, parentTransform);
        //return healthBar;
        //canvas.transform.SetParent(parentTransform);
        //int currentHealth = PlayerHealth.Instance.currentHealth;
        healthBar.SetHealth(currentHealth);
    }
    */
}
