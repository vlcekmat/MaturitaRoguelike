using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemReplace : MonoBehaviour
{
    public Transform[] slots;
    private GameObject[] inventoryItems;
    private GameObject itemToBuy;

    private Transform[] originalPositions;

    private SaveData saveData;

    ItemRandomizer initiator = null;

    public Text comparison;

    private void Start()
    {
        GetComponent<Canvas>().enabled = false;
        saveData = FindObjectOfType<SaveData>();
    }

    public void SetUpReplacement(GameObject toBuy, GameObject[] invItems, ItemRandomizer init)
    {   
        GetComponent<Canvas>().enabled = true;

        initiator = init;
        itemToBuy = toBuy;
        inventoryItems = invItems;

        originalPositions = new Transform[3];

        for(int i = 0; i < slots.Length; i++)
        {   
            invItems[i].GetComponent<Image>().enabled = true;
            invItems[i].transform.SetParent(gameObject.transform);
            originalPositions[i] = invItems[i].transform;
            invItems[i].transform.localPosition = slots[i].localPosition;
        }

        comparison.text = itemToBuy.GetComponent<ItemDisplay>().entry;
    }

    public void CancelReplacement()
    {
        for(int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i].transform.SetParent(null);
            inventoryItems[i].GetComponent<Image>().enabled = false;
            DontDestroyOnLoad(inventoryItems[i]);
        }

        GetComponent<Canvas>().enabled = false;
    }

    public void ReplaceItem(int index)
    {   
        itemToBuy.transform.SetParent(null);
        saveData.gold -= itemToBuy.GetComponent<ItemDisplay>().price;
        saveData.GetComponent<Inventory>().AddItemAtIndex(itemToBuy, index);
        initiator.itemObject.GetComponent<Image>().enabled = false;

        for(int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i].transform.SetParent(null);
            inventoryItems[i].GetComponent<Image>().enabled = false;
            DontDestroyOnLoad(inventoryItems[i]);
        }

        GetComponent<Canvas>().enabled = false;

    }
}
