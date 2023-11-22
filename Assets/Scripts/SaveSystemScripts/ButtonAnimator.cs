using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private float scaleFactor = 0.1f;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Scale the button to a larger size (uses scaleFactor) when cursor is on top
        Vector3 enlargedScale = new Vector3(
            originalScale.x * (1 + scaleFactor),
            originalScale.y * (1 + scaleFactor),
            originalScale.z * (1 + scaleFactor));

        LeanTween.scale(gameObject, enlargedScale, 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Scale the button to its original size when cursor exits range
        LeanTween.scale(gameObject, originalScale, 0.25f);
    }

    void OnDisable()
    {
        // Scale the button to its original size when button gets disabled (on panel change)
        transform.localScale = originalScale;
    }
}