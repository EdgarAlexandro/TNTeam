using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();

    void Start()
    {
        // Disable buttons at the start
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
            //button.onClick.AddListener(() => OnButtonClick(button));
        }
    }

    public void ShowButtons()
    {
        // Enable buttons when called
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
            //button.onClick.AddListener(() => OnButtonClick(button));
        }
    }

    public void OnRestartButtonClicked()
    {
        // Load the game scene
        SceneManager.LoadScene("Main 1");
    }

    public void OnQuitButtonClicked()
    {
        // Quit the application (works in standalone builds)
        SceneManager.LoadScene("StartMenu");
    }
}
