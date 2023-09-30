using UnityEngine;
using UnityEngine.UI;

public class FadeInMessage : MonoBehaviour
{
    public Image gameOverImage;
    public float fadeDuration = 6f;
    public ButtonController buttonHandler;


    private float timer = 0f;

    void Start()
    {
        // Set the initial alpha value to 0
        Color imageColor = gameOverImage.color;
        imageColor.a = 0f;
        gameOverImage.color = imageColor;
    }

    void Update()
    {
        if (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            // Calculate the new alpha value based on time
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);

            // Set the alpha value of the image
            Color imageColor = gameOverImage.color;
            imageColor.a = alpha;
            gameOverImage.color = imageColor;

            if (timer >= fadeDuration)
            {
                buttonHandler.ShowButtons(); // Call ShowButtons when fading is complete
            }
        }
    }
}
