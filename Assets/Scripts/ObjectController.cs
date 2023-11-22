/* Function: controls the behaviour of the available objects (sprite)
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectController : MonoBehaviour
{
    bool reduccion = false;
    public float cambio = 0.01f;
    private PersistenceManager pm = null;
    private DestructionManager dm = null;

    private void Start()
    {
        pm = PersistenceManager.Instance;
        dm = DestructionManager.Instance;
    }
    void FixedUpdate()
    {
        // If the player has touched the object´s sprite
        if (reduccion)
        {
            transform.localScale = new Vector3(transform.localScale.x - cambio, transform.localScale.y - cambio, transform.localScale.z - cambio);
            if (transform.localScale.x < 0.01)
            {
                reduccion = false;
                GetComponentInParent<PhotonView>().RPC("DestroyObject", RpcTarget.All);
            }
        }
    }

    // Checks if the player has touched the object´s sprite and acts according to the object/item
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PhotonView>().IsMine)
        {
            UIController playerUIController = collision.GetComponent<UIController>();
            Inventory playerInventory = collision.GetComponent<InventoryController>().inventory;
            // Limited objects/orbs and unlimited keys
            if (CompareTag("Palanca") || CompareTag("Llave") || (CompareTag("Orbe") && playerUIController.pm.CurrentMagic < 100) || (playerInventory.items.Count < playerInventory.maxItems))
            {
                reduccion = true;
                if (this.CompareTag("Orbe"))
                {
                    collision.gameObject.GetComponent<UIController>().chargeMagicValue(25);
                }
                else if (this.CompareTag("Llave"))
                {
                    if (collision.GetComponent<PhotonView>().IsMine) collision.gameObject.GetComponent<PhotonView>().RPC("IncreaseKeyCount", RpcTarget.All, 1);
                }
                else if (this.CompareTag("Bomba"))
                {
                    collision.gameObject.GetComponent<InventoryController>().inventory.AddItem(collision.GetComponent<InventoryController>().bomba);
                }
                else if (this.CompareTag("Pocion Curacion"))
                {
                    collision.gameObject.GetComponent<InventoryController>().inventory.AddItem(collision.GetComponent<InventoryController>().PC);
                }
                else if (this.CompareTag("Pocion Magia"))
                {
                    collision.gameObject.GetComponent<InventoryController>().inventory.AddItem(collision.GetComponent<InventoryController>().PM);
                }
                else if (this.CompareTag("Pocion Revivir"))
                {
                    collision.gameObject.GetComponent<InventoryController>().inventory.AddItem(collision.GetComponent<InventoryController>().PR);
                }
                else if (this.CompareTag("Palanca"))
                {
                    PersistenceManager.Instance.LeverCounter++;
                    dm.MarkAsDestroyed(gameObject.transform.parent.name);
                }
                collision.gameObject.GetComponent<InventoryController>().UpdateInventory();
            }
        }
    }
}
