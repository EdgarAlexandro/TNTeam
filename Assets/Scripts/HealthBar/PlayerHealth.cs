using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //public int maxHealth;
    //public int currentHealth;
    private Rigidbody2D rb;
    public HealthBar healthBar;
    private SpriteRenderer sprite;
    private PersistenceManager pm;
    
    // Start is called before the first frame update
    void Start()
    {
        pm = PersistenceManager.Instance;
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealth(pm.MaxHealth);
        healthBar.SetHealth(pm.CurrentHealth);
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        pm.CurrentHealth -= damage;
        healthBar.SetHealth(pm.CurrentHealth);
        if (pm.CurrentHealth <= 0)
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
