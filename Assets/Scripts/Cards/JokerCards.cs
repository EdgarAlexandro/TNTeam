using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JokerCards : MonoBehaviour
{
    public GameObject cardMenu; 
    private Vector2 objectPosition;
    private string currentSceneName;
    private DropManager drm;

    // Start is called before the first frame update
    void Start()
    {
        drm = DropManager.Instance;
        objectPosition = transform.position;
        currentSceneName = SceneManager.GetActiveScene().name;
        tag = gameObject.tag;

        // When the object appears, add its position to the drop position list located in the drop manager
        //drm.AddDropPosition(objectPosition, currentSceneName, tag);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(gameObject.name);
            cardMenu.SetActive(true);
        }
    }
}
