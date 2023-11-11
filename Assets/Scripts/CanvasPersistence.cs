/* Function: allows items to not be destroyed on change scene
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 10/11/2023 */
using UnityEngine;

public class CanvasPersistence : MonoBehaviour
{
    private static CanvasPersistence instance = null;
    private void Awake()
    {
        if (gameObject.CompareTag("Canvas general"))
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
