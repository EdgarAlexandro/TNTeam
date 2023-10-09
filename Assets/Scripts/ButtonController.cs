using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    private PersistenceManager pm;
    private DestructionManager dm;
    private DropManager drm;

    void Start()
    {
        pm = PersistenceManager.Instance;
        dm = DestructionManager.Instance;
        drm = DropManager.Instance;
        // Disable buttons at the start
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void ShowButtons()
    {
        // Enable buttons when called
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void OnRestartButtonClicked()
    {
        // Load the game scene and reset values
        SceneManager.LoadScene("Main 1");
        pm.CurrentHealth = 20;
        pm.CurrentMagic = 0;
        pm.CurrentKeys = 0;
        dm.RemoveFromDestroyed();
        drm.RemoveAllDropPositions();
    }

    public void OnQuitButtonClicked()
    {
        // Quit the application 
        Application.Quit();
    }
}
