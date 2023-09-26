using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private Rigidbody2D rb;
    public HealthBar healthBar;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
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
        //rb.velocity = Vector2.zero;
        sprite.color = new Color(255, 255, 255, 255);
    }
}
