using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMisc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite[] sprites; 
    private Image currentSprite;

    public void Start()
    {
        currentSprite = gameObject.GetComponent<Image>();
        currentSprite.sprite = sprites[0];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentSprite.sprite = sprites[1];
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        currentSprite.sprite = sprites[0];
    }
}
