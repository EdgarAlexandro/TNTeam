using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnBasedCardActions : MonoBehaviour
{
    private PersistenceManager pm;
    public TurnBasedCombatPlayersHealth tbcPH;
    private TurnBasedCombatManager tbcm;
    private GameObject healthBars;


    bool prankIsActive;
    bool sentinelIsActive;

    // Start is called before the first frame update
    void Start()
    {
        pm = PersistenceManager.Instance;
        healthBars = GameObject.Find("HealthBars");
        tbcm = GameObject.Find("TurnBasedCombatManager").GetComponent<TurnBasedCombatManager>();
        tbcPH = healthBars.GetComponent<TurnBasedCombatPlayersHealth>();
    }

    // se salta el turno del jugador que usó la carta
    public void Confusion()
    {
        if(PhotonNetwork.OfflineMode)
        {
            tbcm.skipP1Turn = true;
            Debug.Log("Confused player 1");
        }
        else
        {
            tbcm.photonView.RPC("ConfusionHandler", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ConfusionHandler()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            tbcm.skipP1Turn = true;
            Debug.Log("Confused player 1");
        }
        else
        {
            tbcm.skipP2Turn = true;
            Debug.Log("Confused player 2");
        }
    }

    // Activa la variable skipBossTurn, que niega el turno del jefe
    public void NotYet()
    {
        if (PhotonNetwork.OfflineMode)
        {
            tbcm.skipBossTurn = true;
            Debug.Log("Skipped boss turn");
        }
        else
        {
            tbcm.photonView.RPC("SkipBossTurn", RpcTarget.All);
        }
    }

    [PunRPC]
    public void SkipBossTurn()
    {
        tbcm.skipBossTurn = true;
        Debug.Log("Skipped boss turn");
    }

    // aumenta la defensa y el ataque del jefe
    public void JokersPrank()
    {
        if (PhotonNetwork.OfflineMode)
        {
            tbcm.BossAttackMultiplier += 0.25f;
            tbcm.BossDefenseMultiplier += 0.3f;
            Debug.Log("New boss attack/def = " + tbcm.BossAttackMultiplier + "/" + tbcm.BossDefenseMultiplier);
        }
        else
        {
            tbcm.photonView.RPC("UpdateBossMultipliers", RpcTarget.All);
        }
        
    }

    [PunRPC]
    public void UpdateBossMultipliers()
    {
        tbcm.BossAttackMultiplier += 0.25f;
        tbcm.BossDefenseMultiplier += 0.3f;
        Debug.Log("New boss attack/def = " + tbcm.BossAttackMultiplier + "/" + tbcm.BossDefenseMultiplier);
    }

    public void SanaSana() // esta funcion le agrega un 25% de salud a ambos jugadores, por lo que no es necesario revisar quien la uso
    {
        if (PhotonNetwork.OfflineMode)
        {
            if (pm.CurrentHealth + (pm.MaxHealth / 4) <= pm.MaxHealth)
            {
                pm.CurrentHealth += pm.MaxHealth / 4;
            }
            else
            {
                pm.CurrentHealth = pm.MaxHealth;
            }
            Debug.Log("healed player");
        }
        else
        {
            if (pm.CurrentHealth + (pm.MaxHealth/4) <= pm.MaxHealth)
            {
                pm.CurrentHealth += pm.MaxHealth / 4;
            }
            else
            {
                pm.CurrentHealth = pm.MaxHealth;
            }

            // actualiza las health bars
            if (PhotonNetwork.IsMasterClient)
            { 
                tbcPH.photonView.RPC("SyncronizeP1HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
            else
            {
                tbcPH.photonView.RPC("SyncronizeP2HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
            Debug.Log("healed player");
        }
        
    }

    
    public void Sentinel()
    {
        if (PhotonNetwork.OfflineMode)
        {
            tbcm.playerDefenseMultiplier += 0.2f;
            Debug.Log("increased player def");
        }
        else
        {
            tbcm.photonView.RPC("UpdatePlayerDefenseMultiplier", RpcTarget.All);
        }
    }

    [PunRPC]
    public void UpdatePlayerDefenseMultiplier()
    {
        tbcm.playerDefenseMultiplier += 0.2f;
        Debug.Log("increased player def");
    }

    public void MayTheForceBeWithYou()
    {
        if (PhotonNetwork.OfflineMode)
        {
            tbcm.playerAttackMultiplier += 0.3f;
            Debug.Log("increased player attack");
        }
        else
        {
            tbcm.photonView.RPC("UpdatePlayerAttackMultiplier", RpcTarget.All);
        }
    }

    [PunRPC]
    public void UpdatePlayerAttackMultiplier()
    {
        tbcm.playerAttackMultiplier += 0.3f;
        Debug.Log("increased players attack");
    }
}

    
