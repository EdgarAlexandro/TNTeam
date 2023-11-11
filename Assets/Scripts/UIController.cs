/* Function: Control the UI values for all players
   Author: Daniel Degollado Rodr�guez, Edgar Alexandro Castillo Palacios and Carlos Alejandro Morales
   Modification date: 27/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIController : MonoBehaviourPunCallbacks
{
    //public AbrirPuertas abrirPuertas;
    public HealthBar healthBar;
    //private SpriteRenderer sprite;
    public Barradellave keyBar;
    public MagicBar magicBar;
    public PersistenceManager pm;
    public GameObject playerCanvas = null;
    public GameObject canvasGeneral = null;
    private DeathManager dm;
    public bool isDead;
    public GameObject playerGameObject;
    public GameObject cardInventory;

    void Start()
    {
        playerGameObject = gameObject;
        isDead = false;
        pm = PersistenceManager.Instance;
        dm = DeathManager.Instance;

        //initialize player properties ("Magic and Health")
        SetProps();

        if (photonView.IsMine)
        {

            // Assign player canvas depending on player status
            if (PhotonNetwork.IsMasterClient)
            {
                playerCanvas = GameObject.Find("Canvas Player 1");
            }
            else
            {
                playerCanvas = GameObject.Find("Canvas Player 2");
            }
            playerCanvas.SetActive(true);
            // Define general canvas for data both players can see
            canvasGeneral = GameObject.FindGameObjectWithTag("Canvas general");
            //Llaves
            //keyBar = GameObject.Find("Contador de Llave").GetComponent<Barradellave>();
            // Set key values on general canvas
            keyBar = canvasGeneral.GetComponentInChildren<Barradellave>();
            keyBar.SetMaxKeys(pm.MaxKeys);
            keyBar.SetKeys(pm.CurrentKeys);
            keyBar.UpdateText(pm.CurrentKeys);

            //Magia
            //magicBar = GameObject.Find("MagicBar").GetComponent<MagicBar>();
            // Set magic values on player canvas
            magicBar = playerCanvas.GetComponentInChildren<MagicBar>();
            magicBar.SetMaxMagic(pm.MaxMagic);
            magicBar.SetMagic(pm.CurrentMagic);
            magicBar.UpdateText(pm.CurrentMagic);

            //Salud
            //healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
            // Set health values on player canvas
            healthBar = playerCanvas.GetComponentInChildren<HealthBar>();
            healthBar.SetMaxHealth(pm.MaxHealth);
            healthBar.SetHealth(pm.CurrentHealth);

            // Set card inventory section in player canvas in the card inventory controller
            cardInventory = GameObject.Find("CardInventoryController");
            CardInventoryController cardInventoryController = cardInventory.GetComponent<CardInventoryController>();
            cardInventoryController.inventoryView = playerCanvas.transform.Find("CardInventory").gameObject;
            GameObject carsDisplay = cardInventoryController.inventoryView.transform.Find("CardsDisplay").gameObject;
            foreach (Transform child in carsDisplay.transform)
            {
                Button button = child.GetComponent<Button>();
                cardInventoryController.cardDisplayButtons.Add(button);
            }
        }
    }

    //Saves current health and magic in player custom properties for client to read and save data
    public void SetProps()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "Health", pm.CurrentHealth },
            { "Magic", pm.CurrentMagic }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    // Charge magic 
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
            SetProps();
            magicBar.SetMagic(pm.CurrentMagic);
            magicBar.UpdateText(pm.CurrentMagic);
        }
    }
    // Lose magic
    public void loseMagicValue(int value)
    {
        if (pm.CurrentMagic > 0)
        {
            pm.CurrentMagic -= value;
        }
        SetProps();
        magicBar.SetMagic(pm.CurrentMagic);
        magicBar.UpdateText(pm.CurrentMagic);

    }
    // Remote procedure call to increase key value for all players
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
    // Remote procedure call to update the amount of player alive
    [PunRPC]
    public void UpdateAlivePlayers(int count)
    {
        GameController.AlivePlayers = count;
        Debug.Log(GameController.AlivePlayers);
    }
    // Remote procedure call to modify a player's character when they die
    [PunRPC]
    public void DeathHandler()
    {
        // Disabe player control
        PlayerControl playerControl = GetComponent<PlayerControl>();
        playerControl.isActive = false;
        // Disable sprite renderer
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D myCollider = GetComponent<Collider2D>();
        // Set collider to trigger
        myCollider.isTrigger = true;
        // Add constraints to rigidbody
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.simulated = false;
        // Update player's life status
        isDead = true;
    }
    // Remote procedure call to modify a player's character when they revive
    [PunRPC]
    public void RevivalHandler()
    {
        // Enable player control
        PlayerControl playerControl = GetComponent<PlayerControl>();
        playerControl.isActive = true;
        // Enable sprite renderer
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D myCollider = GetComponent<Collider2D>();
        // Set collider to non trigger
        myCollider.isTrigger = false;
        // Remove constraints from rigidbody
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.simulated = true;
        // Update player's life status
        isDead = false;
    }

    // Function to deal damage to the player
    public void TakeDamage(int damage, string player)
    {
        if (pm.CurrentHealth > 0)
        {
            pm.CurrentHealth -= damage;
        }
        healthBar.SetHealth(pm.CurrentHealth);
        SetProps();
        if (pm.CurrentHealth == 0)
        {
            if (PhotonNetwork.OfflineMode)
            {
                Destroy(gameObject);
                NetworkManager.instance.LoadScene("LoseScene");
            }
            else
            {
                PlayerDied();
                if (GameController.AlivePlayers == 0)
                {
                    NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, "LoseScene");
                }
            }
        }
        else
        {
            StartCoroutine(AlternateColors(player));
        }
    }
    /* Call death handler remote procedure call, spawn a thombstone when player dies
    and also callupdate alive players number remote procedure call */
    public void PlayerDied()
    {
        photonView.RPC("DeathHandler", RpcTarget.All);
        dm.SpawnDeadPlayerPrefab(gameObject.transform.position);
        if (GameController.AlivePlayers > 0)
        {
            photonView.RPC("UpdateAlivePlayers", RpcTarget.All, GameController.AlivePlayers - 1);
        }
    }
    /* Call revival handler remote procedure call, call update alive players remote procedure call
     and give 30% of health to the revived player*/
    public void RevivePlayer()
    {
        photonView.RPC("RevivalHandler", RpcTarget.All);
        photonView.RPC("UpdateAlivePlayers", RpcTarget.All, GameController.AlivePlayers + 1);
        pm.CurrentHealth += (int)(pm.MaxHealth * .30f);
        healthBar.SetHealth(pm.CurrentHealth);
    }
    // Remote procedure call to change player's color
    [PunRPC]
    public void ChangeColor(string player)
    {
        GameObject playerGO = GameObject.Find(player);
        playerGO.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
    }
    // Remote procedure call to restore player's color
    [PunRPC]
    public void ReturnColor(string player)
    {
        GameObject playerGO = GameObject.Find(player);
        playerGO.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }
    // Coroutine to alternate colors when players take damage
    public IEnumerator AlternateColors(string player)
    {
        photonView.RPC("ChangeColor", RpcTarget.All, player);
        yield return new WaitForSeconds(0.2f);
        photonView.RPC("ReturnColor", RpcTarget.All, player);
    }
}
