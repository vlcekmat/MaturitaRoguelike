using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRandomizer : MonoBehaviour
{   

    SaveData saveData;
    public Canvas replaceCanvas;

    // randomization settings
    public Sprite[] availibleSprites;
    public float[] maxHealthEffectRange = {0, 250};
    public float[] damageEffectRange = {0, 20};
    public float[] speedEffectRange = {0, 2};
    public int[] priceRange = {20, 2000};
    public GameObject itemPrefab;

    // parameters for the generated object
    private new string name = "";
    private int price = 10;

    private float maxHealthEffect = 0;
    private float damageEffect = 0;
    private float speedEffect = 0;
    private Sprite sprite;

    // completed object
    public GameObject itemObject;

    private void Start()
    {   
        saveData = FindObjectOfType<SaveData>();
        RandomizeParameters();
        CreateItemObject();
    }

    private void RandomizeParameters()
    {   
        // calculate how many attributes should be affected
        int affected = (int)Random.Range(1, 3);

        // which particular attributes should be affected; 0 - maxHealth, 1 - damage, 2 - speed
        List<int> choices = new List<int>();
        choices.Add(0);
        choices.Add(1);
        choices.Add(2);

        List<int> chosen = new List<int>();

        for(int i = 0; i < affected; i++)
        {   
            int index = (int)Random.Range(0, choices.Count);
            chosen.Add(choices[index]);
            choices.RemoveAt(index);
        }

        // picks random values for effects of the item
        for(int j = 0; j < chosen.Count; j++)
        {
            if(chosen[j] == 0)
            {
                maxHealthEffect = Random.Range(maxHealthEffectRange[0], maxHealthEffectRange[1]);
            }
            else if(chosen[j] == 1)
            {
                damageEffect = Random.Range(damageEffectRange[0], damageEffectRange[1]);
            }
            else if(chosen[j] == 2)
            {
                speedEffect = Random.Range(speedEffectRange[0], speedEffectRange[1]);
            }
        }


        // picks a random price based on the given rules
        price = (int)((affected*((int)maxHealthEffect + (int)(damageEffect*10) + (int)(speedEffect*100))) * Random.Range(0.7f, 1.5f));

        // picks a random sprite from the sprite bank
        sprite = availibleSprites[(int)Random.Range(0, availibleSprites.Length)];

        name = RandomizeName();
    }

    private string RandomizeName()
    {
        string[] words = 
        {
            "Damnation", "Jinx", "Voodoo", "Toadstone", "Mojo", "Madstone", "Juju",
            "Mumbo Jumbo", "Eternity", "Salvation", "Bamboozle", "Equilibrium", 
            "the Sinister", "Virgo", "Cancer", "Scorpio", "Aquarius", "Aries", "Taurus", 
            "Gemini", "Leo", "Libra", "Sagittarius", "Capricorn", "Pisces", "Reprobation",
            "Drowning", "Ramesse", "Rudoplhine", "Victoria", "the Dead", "the Forsaken", 
            "the Eternal", "the Ancestors", "Synthesis", "the Creation", "Bravery", "Glory",
            "the Past", "Mourning"
        };

        string firstWord = "Artifact";
        if(sprite == availibleSprites[0])
        {
            firstWord = "Goblet";
        }
        else if(sprite == availibleSprites[1])
        {
            firstWord = "Necklace";
        }
        else if(sprite == availibleSprites[2])
        {
            firstWord = "Orb";
        }

        return firstWord + " of " + words[(int)Random.Range(0, words.Length)];
    }

    private void CreateItemObject()
    {   
        itemObject = Instantiate (itemPrefab, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity) as GameObject;

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

        itemObject.transform.SetParent(properCanvas.gameObject.transform);
        
        ItemDisplay itemDisplay = itemObject.GetComponent<ItemDisplay>();
        itemDisplay.name = name;
        itemDisplay.price = price;
        itemDisplay.sprite = sprite;
        itemDisplay.onMaxHealth = (int)maxHealthEffect;
        itemDisplay.onDamage = (int)damageEffect;
        itemDisplay.onSpeed = speedEffect;
    }

    public void BuyObject()
    {
        Inventory inventory = saveData.GetComponent<Inventory>();
        ItemDisplay itemToBuy = itemObject.GetComponent<ItemDisplay>();
        if (itemObject.activeSelf && itemToBuy.price <= saveData.gold && inventory.HasFreeSlot())
        {   
            // A free slot found, the item can be added to the inventory
            itemToBuy.gameObject.transform.SetParent(null);
            DontDestroyOnLoad(itemToBuy.gameObject);
            saveData.gold -= itemToBuy.price;
            inventory.AddItem(itemToBuy.gameObject);
            itemObject.GetComponent<Image>().enabled = false;
        }
        else if(itemObject.activeSelf && itemToBuy.price <= saveData.gold && !inventory.HasFreeSlot())
        {   
            // No more free slots, an item must be replaced
            // Proceed to the replace window
            replaceCanvas.GetComponent<ItemReplace>().SetUpReplacement(itemToBuy.gameObject, inventory.items, this);
        } 
    }
}
