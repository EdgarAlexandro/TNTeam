using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class QueenDaggerThrow : MonoBehaviourPunCallbacks
{
    public GameObject dagger;
    public float speed = 3;
    public int neededMagic = 10;
    private Vector2 projDirection = Vector2.down;
    public UIController uiController;
    private MusicSFXManager musicSFXManager;

    // Start is called before the first frame update
    void Start()
    {
        projDirection = Vector2.down;
        musicSFXManager = MusicSFXManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        

        if (Mathf.Abs(xInput) > Mathf.Abs(yInput))
        {
            // Movimiento horizontal
            if (xInput > 0)
            {
                projDirection = Vector2.right;
            }
            else if (xInput < 0)
            {
                projDirection = Vector2.left;
            }
        }
        else
        {
            // Movimiento vertical
            if (yInput > 0)
            {
                projDirection = Vector2.up;
            }
            else if (yInput < 0)
            {
                projDirection = Vector2.down;
            }
        }

        if (photonView.IsMine && Input.GetKeyDown(KeyCode.R)) 
        {
            if (PersistenceManager.Instance.CurrentMagic >= neededMagic)
            {
                musicSFXManager.PlaySFX(MusicSFXManager.Instance.Lanza_Daga);
                uiController.loseMagicValue(neededMagic);
                photonView.RPC("throwDagger", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void throwDagger()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject daggerInstanceObject = PhotonNetwork.Instantiate(dagger.name, transform.position, Quaternion.identity);
            daggerInstanceObject.GetComponent<HeartDagger>().photonView.RPC("MoveDagger", RpcTarget.All, projDirection, speed); 
        }
    }
}
