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

    // Update is called once per frame
    /*void Update()
    {
        
    }*/


    
    public void JokersPrank()
    {

    }

    [PunRPC]
    public void UpdateBossMultipliers()
    {
        tbcm.BossAttackMultiplier += 0.25f;
        tbcm.BossDefenseMultiplier += 0.3f;

    }

    public void SanaSana() // esta funcion le agrega un 25% de salud a ambos jugadores, por lo que no es necesario revisar quien la uso
    {
        // revisa si agregar la vida sobrepasa la vida maxima y agrega la vida acorde
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
    }

    
    public void Sentinel()
    {
        tbcm.photonView.RPC("updateplayerdefensemultiplier", RpcTarget.All);
    }

    [PunRPC]
    public void UpdatePlayerDefenseMultiplier()
    {
        tbcm.playerDefenseMultiplier += 0.2f;
    }

}
