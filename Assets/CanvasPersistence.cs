using UnityEngine;

public class CanvasPersistence : MonoBehaviour
{
    private static CanvasPersistence instance = null;
    private void Awake()
    {
        if (gameObject.name == "Canvas general")
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
