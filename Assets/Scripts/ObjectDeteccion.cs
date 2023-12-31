/* Function: controls the behaviour of the available objects (detection radius)
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ObjectDeteccion : MonoBehaviourPunCallbacks
{
    public Rigidbody2D rig = null;
    private Transform jugador = null;
    public float velocidadMovimiento = 0.1f;
    private Vector2 objectPosition = new();
    private string currentSceneName = null;
    private DropManager drm = null;
    private PersistenceManager pm = null;

    // Remote Procedure that destroys the object for all players
    [PunRPC]
    public void DestroyObject()
    {
        // Remove the objects position from the drop position list located in the drop manager
        if (drm.IsDropped(objectPosition, currentSceneName, tag))
        {
            drm.RemoveDropPosition(objectPosition, currentSceneName, tag);
        }
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void Start()
    {
        drm = DropManager.Instance;
        pm = PersistenceManager.Instance;
        objectPosition = transform.position;
        currentSceneName = SceneManager.GetActiveScene().name;
        tag = gameObject.tag;
        jugador = null;

        // When the object appears, add its position to the drop position list located in the drop manager
        drm.AddDropPosition(objectPosition, currentSceneName, tag);
    }

    void Update()
    {
        if (jugador != null)
        {
            // Move the orb towards the player
            velocidadMovimiento = 0.1f;
            Vector2 nuevaPosicion = Vector2.Lerp(transform.position, jugador.position, velocidadMovimiento);
            rig.MovePosition(nuevaPosicion);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            UIController playerUIController = other.GetComponent<UIController>();
            Inventory playerInventory = other.GetComponent<InventoryController>().inventory;
            if (CompareTag("Palanca") || CompareTag("Llave") || (CompareTag("Orbe") && playerUIController.pm.CurrentMagic < 100) || (playerInventory.items.Count < playerInventory.maxItems))
            {
                // Sets the variable as the object that entered the trigger's transform
                jugador = other.transform;
                Invoke("stopFollowing", 0.4f);
            }
        }
    }

    // Function to stop following after some time has passed
    void stopFollowing()
    {
        jugador = null;
    }

}
