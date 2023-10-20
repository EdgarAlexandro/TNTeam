using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIController : MonoBehaviourPunCallbacks
{
    //public AbrirPuertas abrirPuertas;
    public HealthBar healthBar;
    private SpriteRenderer sprite;
    public Barradellave keyBar;
    public MagicBar magicBar;
    private PersistenceManager pm;
    public GameObject playerCanvas = null;
    public GameObject canvasGeneral = null;

    void Start()
    {
        pm = PersistenceManager.Instance;

        if (photonView.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerCanvas = GameObject.Find("Canvas Player 1");
            }
            else
            {
                playerCanvas = GameObject.Find("Canvas Player 2");
            }
            playerCanvas.SetActive(true);
            canvasGeneral = GameObject.Find("Canvas general");
            //Llaves
            //keyBar = GameObject.Find("Contador de Llave").GetComponent<Barradellave>();
            keyBar = canvasGeneral.GetComponentInChildren<Barradellave>();
            keyBar.SetMaxKeys(pm.MaxKeys);
            keyBar.SetKeys(pm.CurrentKeys);
            keyBar.UpdateText(pm.CurrentKeys);

            //Magia
            //magicBar = GameObject.Find("MagicBar").GetComponent<MagicBar>();
            magicBar = playerCanvas.GetComponentInChildren<MagicBar>();
            magicBar.SetMaxMagic(pm.MaxMagic);
            magicBar.SetMagic(pm.CurrentMagic);
            magicBar.UpdateText(pm.CurrentMagic);

            //Salud
            //healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
            healthBar = playerCanvas.GetComponentInChildren<HealthBar>();
            healthBar.SetMaxHealth(pm.MaxHealth);
            healthBar.SetHealth(pm.CurrentHealth);

            sprite = GetComponent<SpriteRenderer>();
        }

    }

    public void chargeMagicValue(int value)
    {
        if (photonView.IsMine)
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

    [PunRPC]
    public void IncreaseKeyCount(int value)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            UIController uiPlayer = p.GetComponent<UIController>();
            if (p.GetComponent<PhotonView>().IsMine && uiPlayer.pm.CurrentKeys < uiPlayer.pm.MaxKeys)
            {
                uiPlayer.pm.CurrentKeys += value;
                uiPlayer.keyBar.SetKeys(uiPlayer.pm.CurrentKeys);
                uiPlayer.keyBar.UpdateText(uiPlayer.pm.CurrentKeys);
            }
        }
    }

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

    public void TakeDamage(int damage, string player)
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
            StartCoroutine(AlternateColors(player));
        }
    }

    public void PlayerDied()
    {
        photonView.RPC("DeathHandler", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ChangeColor(string player)
    {
        GameObject playerGO = GameObject.Find(player);
        playerGO.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
    }

    [PunRPC]
    public void ReturnColor(string player)
    {
        GameObject playerGO = GameObject.Find(player);
        playerGO.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

    public IEnumerator AlternateColors(string player)
    {
        photonView.RPC("ChangeColor", RpcTarget.All, player);
        yield return new WaitForSeconds(0.2f);
        photonView.RPC("ReturnColor", RpcTarget.All, player);
    }
}
