using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{   
    public float x_offset = 0f;
    public float y_offset = 0f;

    private Text text;

    void Awake()
    {   
        // A simple singleton

        int tooltipCount = FindObjectsOfType<Tooltip>().Length;
        if (tooltipCount > 1)
        {   
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject.transform.parent);
        }
    }

    void Start()
    {   
        text = gameObject.GetComponentInChildren<Text>();
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.GetComponentInChildren<Text>().enabled = false;
    }
    void Update()
    {
        transform.localPosition = 
        new Vector3(Input.mousePosition.x + x_offset, Input.mousePosition.y + y_offset, Input.mousePosition.z);
    }

    public void ShowTooltip(string entry)
    {   
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.GetComponentInChildren<Text>().enabled = true;
        text.text = "";
        text.text = entry;
    }

    public void HideTooltip()
    {
        text.text = "";
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.GetComponentInChildren<Text>().enabled = false;
    }
}
