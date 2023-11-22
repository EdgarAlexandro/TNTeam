/* Function: uses LeanTween to scale the logo in loading screen
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 17/11/2023 */

using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    // Scale values
    // Set the target scale values for large and small states
    private Vector3 largeScale = new(1.5f, 1.5f, 1.5f);
    private Vector3 smallScale = new(1f, 1f, 1f);

    void Awake()
    {
        // Start the loop animation
        ScaleLarge();
    }

    void ScaleLarge()
    {
        // Scale the object to a larger size
        LeanTween.scale(gameObject, largeScale, 1f).setOnComplete(ScaleSmall);
    }

    void ScaleSmall()
    {
        // Scale the object back to its smaller size
        LeanTween.scale(gameObject, smallScale, 1f).setOnComplete(ScaleLarge);
    }
}
