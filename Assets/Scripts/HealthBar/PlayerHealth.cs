using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private Rigidbody2D rb;
    public HealthBar healthBar;
    private SpriteRenderer sprite;
    
    /*
    private static PlayerHealth instance;
    public static PlayerHealth Instance { get { return instance; } }

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
    
    */
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //healthBar = PersistenceManager.Instance.CreateHealthBar(transform);
        healthBar.SetMaxHealth(maxHealth);
        //PersistenceManager.Instance.AlterHealthBar();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        //PersistenceManager.Instance.AlterHealthBar();
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("LoseScene");
        }
        else
        {
            sprite.color = new Color(255, 0, 0, 255);
            StartCoroutine(ReturnToNormalColor());
           
        }
    }

    private IEnumerator ReturnToNormalColor()
    {
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(255, 255, 255, 255);
    }
}
