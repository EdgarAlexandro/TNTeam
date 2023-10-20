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
    private GameObject imagenCanvasDetras = null;
    private GameObject imagenCanvasCentro = null;
    private GameObject imagenCanvasDelante = null;
    public Sprite empty = null;
    private PersistenceManager pm = null;

    private void Start()
    {
        pm = PersistenceManager.Instance;
        imagenCanvasDetras = GameObject.Find("ItemImageSpaceBack");
        imagenCanvasCentro = GameObject.Find("ItemImageSpaceCenter");
        imagenCanvasDelante = GameObject.Find("ItemImageSpaceFront");
    }

    private void Update()
    {
        // Keeps index in range
        if (indexItemSeleccionado >= inventory.items.Count) indexItemSeleccionado = 0;
        // Changes the canvas item image
        if (imagenCanvasCentro != null && imagenCanvasDelante != null && imagenCanvasDetras != null && photonView.IsMine)
        {
            int indexDelante = indexItemSeleccionado + 1;
            if (indexDelante > inventory.items.Count - 1) indexDelante = 0;
            int indexDetras = indexItemSeleccionado - 1;
            if (indexDetras < 0) indexDetras = inventory.items.Count - 1;
            if (inventory.items.Count == 0)
            {
                imagenCanvasDetras.SetActive(false);
                imagenCanvasCentro.GetComponent<Image>().sprite = empty;
                imagenCanvasDelante.SetActive(false);
            }
            if (inventory.items.Count == 1)
            {
                imagenCanvasDetras.SetActive(false);
                imagenCanvasCentro.GetComponent<Image>().sprite = inventory.items[indexItemSeleccionado].itemImage;
                imagenCanvasDelante.SetActive(false);
            }
            if (inventory.items.Count == 2)
            {
                imagenCanvasDetras.GetComponent<Image>().sprite = inventory.items[indexDetras].itemImage;
                imagenCanvasCentro.GetComponent<Image>().sprite = inventory.items[indexItemSeleccionado].itemImage;

                imagenCanvasDetras.GetComponentInParent<HorizontalLayoutGroup>().spacing = -20;
                imagenCanvasDetras.SetActive(true);
                imagenCanvasDelante.SetActive(false);
            }
            if (inventory.items.Count >= 3)
            {
                imagenCanvasDetras.GetComponent<Image>().sprite = inventory.items[indexDetras].itemImage;
                imagenCanvasCentro.GetComponent<Image>().sprite = inventory.items[indexItemSeleccionado].itemImage;
                imagenCanvasDelante.GetComponent<Image>().sprite = inventory.items[indexDelante].itemImage;
                imagenCanvasDelante.GetComponentInParent<HorizontalLayoutGroup>().spacing = -80;
                imagenCanvasDetras.SetActive(true);
                imagenCanvasDelante.SetActive(true);
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
                        GetComponent<UIController>().healthBar.SetHealth(pm.CurrentHealth);
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
