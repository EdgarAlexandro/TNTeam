/* Function: controls the behaviour of the available objects (detection radius)
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDeteccion : MonoBehaviour
{
    public Rigidbody2D rig = null;
    private Transform jugador = null;
    public float velocidadMovimiento = 0.1f;
    private Vector2 objectPosition = new();
    private string currentSceneName = null;
    private DropManager drm = null;

    void Start()
    {
        drm = DropManager.Instance;
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
            Vector2 nuevaPosicion = Vector2.Lerp(transform.position, jugador.position, velocidadMovimiento * Time.deltaTime);
            rig.MovePosition(nuevaPosicion);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if ((other.CompareTag("Player") && this.CompareTag("Orbe")) || (other.CompareTag("Player") && other.GetComponent<InventoryController>().inventory.items.Count < other.GetComponent<InventoryController>().inventory.maxItems))
        {
            // Sets the variable as the object that entered the trigger's transform
            jugador = other.transform;
            Invoke("stopFollowing", 0.4f);
            // Remove the objects position from the drop position list located in the drop manager
            drm.RemoveDropPosition(objectPosition, currentSceneName, tag);
        }
    }

    // Function to stop following after some time has passed
    void stopFollowing()
    {
        jugador = null;
    }

}
