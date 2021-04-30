using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{   
    public GameObject[] items;
    public Transform[] itemSlots;

    private void Start()
    {
        items = new GameObject[3];
    }

    public void AddItem(GameObject item)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                items[i] = item;
                if(itemSlots[0] != null)
                {
                    item.transform.position = Camera.main.WorldToScreenPoint(itemSlots[i].transform.position);
                }
                break;
            }
        }
    } 

    public void AddItemAtIndex(GameObject item, int index)
    {   
        if(items[index] != null)
        {
            Destroy(items[index]);
        }
        items[index] = item;
        if(itemSlots[0] != null)
        {
            item.transform.position = Camera.main.WorldToScreenPoint(itemSlots[index].transform.position);
        }
    }

    public void RemoveItemOfIndex(int index)
    {   
        if(items[index] != null)
        {
            Destroy(items[index]);
            items[index] = null;
        }
    }

    public void MakeAllVisible()
    {
        for(int i = 0; i < items.Length; i++)
        {   
            if(items[i] != null)
            {   
                Canvas properCanvas = FindObjectOfType<Canvas>();
                Canvas[] options = FindObjectsOfType<Canvas>();
                for(int j = 0; j < options.Length; j++)
                {
                    if(options[j].name == "Canvas")
                    {
                        properCanvas = options[j];
                        break;
                    }
                }
                items[i].transform.localPosition = Camera.main.WorldToScreenPoint(itemSlots[i].transform.position);
                items[i].transform.SetParent(properCanvas.gameObject.transform);
                items[i].GetComponent<Image>().enabled = true;
            }
        }
    }

    public bool HasFreeSlot()
    {
        bool freeSlot = false;

        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                freeSlot = true;
            }
        }

        return freeSlot;
    }



}
