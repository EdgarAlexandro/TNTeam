/* Function: controls the behaviour of the inventory and allows player to manage it
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InventoryController : MonoBehaviourPunCallbacks
{
    public Inventory inventory = null;
    public ItemData bomba = null;
    public ItemData PC = null;
    public ItemData PR = null;
    public ItemData PM = null;
    public int indexItemSeleccionado = 0;
    private GameObject imagenCanvas = null;
    public Sprite empty = null;
    private PersistenceManager pm = null;

    private void Start()
    {
        pm = PersistenceManager.Instance;
        imagenCanvas = GameObject.Find("ItemImageSpace");
    }

    private void Update()
    {
        // Keeps index in range
        if (indexItemSeleccionado >= inventory.items.Count) indexItemSeleccionado = 0;
        // Changes the canvas item image
        if (imagenCanvas != null)
        {
            if (inventory.items.Count == 0)
            {
                imagenCanvas.GetComponent<Image>().sprite = empty;
            }
            else
            {
                imagenCanvas.GetComponent<Image>().sprite = inventory.items[indexItemSeleccionado].itemImage;
            }
        }

        // Change selected item
        if (Input.GetKeyDown(KeyCode.Plus) && photonView.IsMine)
        {
            indexItemSeleccionado++;
        }

        // Drop item
        if (Input.GetKeyDown(KeyCode.U) && inventory.items.Count > 0 && photonView.IsMine)
        {
            /* Vector2 projDirection = Vector2.down;
            Vector3 offset = new(0f, 2.5f, 1.8f);
            if (Mathf.Abs(xInput) > Mathf.Abs(yInput))
            {
                if (xInput > 0)
                {
                    offset = new(2.5f, 0f, 1.8f);
                    projDirection = Vector2.right;
                }
                else if (xInput < 0)
                {
                    offset = new(-2.5f, 0f, 1.8f);
                    projDirection = Vector2.left;
                }
            }
            else
            {
                if (yInput > 0)
                {
                    offset = new(0f, 2.5f, 1.8f);
                    projDirection = Vector2.up;
                }
                else if (yInput < 0)
                {
                    offset = new(0f, -2.5f, 1.8f);
                    projDirection = Vector2.down;
                }
            }
            */
            Vector3 offset = new(1.5f, 1.5f, 1.5f);
            offset += gameObject.transform.position;
            inventory.DropItem(indexItemSeleccionado, offset);
        }

        // Use item
        if (Input.GetKeyDown(KeyCode.I) && inventory.items.Count > 0 && photonView.IsMine)
        {
            switch (inventory.items[indexItemSeleccionado].itemName)
            {
                case "Pocion Curacion":
                    if ((pm.CurrentHealth + pm.MaxHealth * .25f) <= pm.MaxHealth)
                    {
                        pm.CurrentHealth += (int)(pm.MaxHealth * .25f);
                        GetComponent<PlayerHealth>().healthBar.SetHealth(pm.CurrentHealth);
                        inventory.UseItem(indexItemSeleccionado);
                    }
                    break;

                case "Pocion Magia":
                    if ((pm.CurrentMagic + pm.MaxMagic * .25f) <= pm.MaxMagic)
                    {
                        pm.CurrentMagic += (int)(pm.MaxMagic * .3f);
                        GetComponent<UIController>().magicBar.SetMagic(pm.CurrentMagic);
                        GetComponent<UIController>().magicBar.UpdateText(pm.CurrentMagic);
                        inventory.UseItem(indexItemSeleccionado);
                    }
                    break;
                case "Bomba":
                    Vector3 offset = new(1.5f, 1.5f, 1.5f);
                    offset += gameObject.transform.position;
                    PhotonNetwork.Instantiate(inventory.items[indexItemSeleccionado].prefabs[1], offset, Quaternion.identity);
                    inventory.UseItem(indexItemSeleccionado);
                    break;

                default:
                    break;
            }
        }
    }
}
