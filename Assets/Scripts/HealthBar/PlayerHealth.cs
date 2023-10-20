using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    //public int maxHealth;
    //public int currentHealth;
    private Rigidbody2D rb;
    public HealthBar healthBar;
    private SpriteRenderer sprite;
    private PersistenceManager pm;

    [PunRPC]
    public void DeathHandler()
    {
        // Freeze player position and disable movement controls
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Collider2D>().enabled = false;

        // Change sprite color to gray
        sprite.color = Color.gray;

        if (GameController.AlivePlayers > 0)
        {
            GameController.AlivePlayers--;
        }  
    }

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
            if (PhotonNetwork.OfflineMode)
            {
                Destroy(gameObject);
            }else
            {
                PlayerDied();               
                Debug.Log(GameController.AlivePlayers);
                if (GameController.AlivePlayers == 0)
                {
                    NetworkManager.instance.LoadScene("LoseScene");
                }
            }
        }
        else
        {
            sprite.color = new Color(255, 0, 0, 255);
            StartCoroutine(ReturnToNormalColor());
        }
    }

    public void PlayerDied()
    {
        photonView.RPC("DeathHandler", RpcTarget.AllBuffered);
    }

    private IEnumerator ReturnToNormalColor()
    {
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(255, 255, 255, 255);
    }
}
