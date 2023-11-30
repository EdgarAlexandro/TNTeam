using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DiamondSpecialArrow : MonoBehaviourPunCallbacks
{
    public Rigidbody2D arrow;
    public float speed = 3;
    private Vector2 projDirection = Vector2.down;
    private MusicSFXManager musicSFXManager;
    public UIController uiController;
    public int neededMagic = 10;


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
                //musicSFXManager.PlaySFX(MusicSFXManager.Instance.TemporaryArrow);
                Rigidbody2D arrowInstance = Instantiate(arrow, transform.position, Quaternion.identity);
                arrowInstance.velocity = projDirection * speed;

                // Rotar la flecha para que apunte en la direcci�n correcta
                float angle = Mathf.Atan2(projDirection.y, projDirection.x) * Mathf.Rad2Deg;
                arrowInstance.rotation = angle;

                uiController.loseMagicValue(neededMagic);
                photonView.RPC("MagicArrow", RpcTarget.All);

            }
        }
    
        /*if (Input.GetKeyDown(KeyCode.R)){
            if (PersistenceManager.Instance.CurrentMagic >= neededMagic)
            {
                Rigidbody2D arrowInstance = Instantiate(arrow, transform.position, Quaternion.identity);
                arrowInstance.velocity = projDirection * speed;
                musicSFXManager.PlaySFX(MusicSFXManager.Instance.Arco);
                uiController.loseMagicValue(neededMagic);
            }
             
        }*/
    }
    [PunRPC]
    public void MagicArrow()
    {
        GameObject arrowInstanceObject = PhotonNetwork.Instantiate(arrow.name, transform.position, Quaternion.identity);
        Rigidbody2D daggerInstance = arrowInstanceObject.GetComponent<Rigidbody2D>();
        daggerInstance.velocity = projDirection * speed;
    }
}




