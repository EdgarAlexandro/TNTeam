using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color hoverColor;
    private Color originalColor;
    private Button button;
    private bool isSelected;

    void Start()
    {
        isSelected = false;
        button = GetComponent<Button>();
        originalColor = button.colors.normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            GameObject child = gameObject.transform.GetChild(0).gameObject;
            child.GetComponent<CardBehaviour>().ChangeCardsColor(hoverColor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            GameObject child = gameObject.transform.GetChild(0).gameObject;
            child.GetComponent<CardBehaviour>().ChangeCardsColor(originalColor);
        }
    }

    public void CardStatus(bool status)
    {
        isSelected = status;
    }
}
