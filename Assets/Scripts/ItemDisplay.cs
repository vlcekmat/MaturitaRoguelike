using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{   
    public new string name;
    public Sprite sprite;
    public Tooltip tooltip;

    public int price = 10;

    // effects on stats
    public int onMaxHealth = 0;
    public int onDamage = 0;
    public float onSpeed = 0;

    public string entry;
    void Start()
    {   
        entry = string.Format(
            "Name: {0}\nPrice: {1} gold\n\nMax Health: +{2}\nDamage: +{3}\nSpeed: +{4}", name, price, onMaxHealth, onDamage, onSpeed);
        tooltip = FindObjectOfType<Tooltip>();
        if(sprite != null)
        {
            gameObject.GetComponent<Image>().sprite = sprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(entry);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

}
