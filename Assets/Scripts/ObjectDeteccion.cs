using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDeteccion : MonoBehaviour
{
    public Rigidbody2D rig;
    private Transform jugador;
    public float velocidadMovimiento = 0.1f;
    private Vector2 objectPosition;
    private string currentSceneName;
    private DropManager drm;

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
        // Check if the variable is not null
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

    void stopFollowing()
    {
        jugador = null;
    }

}
