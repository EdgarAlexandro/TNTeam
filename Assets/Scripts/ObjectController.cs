using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    bool reduccion = false;
    public float cambio = 0.01f;

    void FixedUpdate()
    {
        if (reduccion)
        {
            transform.localScale = new Vector3(transform.localScale.x - cambio, transform.localScale.y - cambio, transform.localScale.z - cambio);
            if (transform.localScale.x < 0.01)
            {
                reduccion = false;
                Destroy(transform.parent.gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") && this.CompareTag("Orbe")) || (collision.CompareTag("Player") && collision.GetComponent<InventoryController>().inventory.items.Count < collision.GetComponent<InventoryController>().inventory.maxItems))
        {
            reduccion = true;
            if (this.CompareTag("Orbe"))
            {
                collision.gameObject.GetComponent<UIController>().chargeMagicValue(25);
            }
            else if (this.CompareTag("Llave"))
            {
                collision.gameObject.GetComponent<UIController>().increaseKeyCount(1);
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

        }
    }
}
