using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ObjectsMenuButton : MonoBehaviourPunCallbacks
{
    private int uses;
    public Button button;
    public TextMeshProUGUI itemCountText;
    PersistenceManager pm;
    TurnBasedCombatPlayersHealth tbcPH;
    private GameObject healthBars;
    // Start is called before the first frame update
    void Start()
    {
        pm = PersistenceManager.Instance;
        healthBars = GameObject.Find("HealthBars");
        tbcPH = healthBars.GetComponent<TurnBasedCombatPlayersHealth>();
        //button = gameObject.GetComponent<Button>();
    }

    public void GetNumberOfUses(int numberOfUses)
    {
        uses = numberOfUses;
        itemCountText.text = uses.ToString();
        UpdateButtonStatus();
    }

    void UpdateButtonStatus()
    {
        if(uses > 0)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }


    public void PocionCuracion()
    {
        uses--;
        itemCountText.text = uses.ToString();
        if (PhotonNetwork.IsMasterClient)
        {
            if(pm.CurrentHealth + 10 < tbcPH.p1HealthBar.maxValue)
            {
                pm.CurrentHealth += 10;
                tbcPH.photonView.RPC("SyncronizeP1HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
            else
            {
                pm.CurrentHealth += (int)tbcPH.p1HealthBar.maxValue - pm.CurrentHealth;
                tbcPH.photonView.RPC("SyncronizeP1HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
        }
        else
        {
            if (pm.CurrentHealth + 10 < tbcPH.p2HealthBar.maxValue)
            {
                pm.CurrentHealth += 10;
                tbcPH.photonView.RPC("SyncronizeP2HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
            else
            {
                pm.CurrentHealth += (int)tbcPH.p1HealthBar.maxValue - pm.CurrentHealth;
                tbcPH.photonView.RPC("SyncronizeP2HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
        }
        UpdateButtonStatus();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
