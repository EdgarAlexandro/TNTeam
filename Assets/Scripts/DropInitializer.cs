using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropInitializer : MonoBehaviour
{
    private DropManager drm;

    // Start is called before the first frame update
    void Start()
    {
        drm = DropManager.Instance;
        string currentSceneName = SceneManager.GetActiveScene().name;

        // If there are saved drop positions, restore the drops in their corresponding position.
        // The current scene is given as a parameter to ensure the orbs are restored in the corrrect scene
        if (drm.HasSavedDropPositions())
        {
            drm.RestoreDropPositions(currentSceneName);
        }
    }
}
