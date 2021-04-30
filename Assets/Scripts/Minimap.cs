using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject roomSymbol;
    public GameObject pathHorizontal;
    public GameObject pathVertical;
    private Vector3 originalCamPosition;

    void Start()
    {   
        transform.localPosition = new Vector3(-2000, 0, -5);
        originalCamPosition = transform.localPosition;
        PlaceRoom();
    }

    public void PlaceRoom()
    {
        GameObject symbol = Instantiate(
            roomSymbol, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1), Quaternion.identity
            );
    }

    // 0 = UP, 1 = DOWN, 2 = LEFT, 3 = RIGHT
    public void CreatePath(int direction)
    {
        if(direction == 0)
        {
            transform.localPosition = transform.localPosition + new Vector3(0, 1, 0);
            Instantiate(pathVertical, transform.localPosition + new Vector3(0, 0, 1), Quaternion.identity);
            transform.localPosition = transform.localPosition + new Vector3(0, 1, 0);

            PlaceRoom();
        }
        else if(direction == 1)
        {
            transform.localPosition = transform.localPosition + new Vector3(0, -1, 0);
            Instantiate(pathVertical, transform.localPosition + new Vector3(0, 0, 1), Quaternion.identity);
            transform.localPosition = transform.localPosition + new Vector3(0, -1, 0);

            PlaceRoom();
        }
        else if(direction == 2)
        {
            transform.localPosition = transform.localPosition + new Vector3(-1, 0, 0);
            Instantiate(pathHorizontal, transform.localPosition + new Vector3(0, 0, 1), Quaternion.identity);
            transform.localPosition = transform.localPosition + new Vector3(-1, 0, 0);

            PlaceRoom();
        }

        else if(direction == 3)
        {
            transform.localPosition = transform.localPosition + new Vector3(1, 0, 0);
            Instantiate(pathHorizontal, transform.localPosition + new Vector3(0, 0, 1), Quaternion.identity);
            transform.localPosition = transform.localPosition + new Vector3(1, 0, 0);

            PlaceRoom();
        }
    }

    public void ResetCam()
    {
        transform.localPosition = originalCamPosition;
    }

    public void Move(int direction)
    {
        if(direction == 0)
        {
            transform.localPosition = transform.localPosition + new Vector3(0, 1, 0);
            transform.localPosition = transform.localPosition + new Vector3(0, 1, 0);
        }
        else if(direction == 1)
        {
            transform.localPosition = transform.localPosition + new Vector3(0, -1, 0);
            transform.localPosition = transform.localPosition + new Vector3(0, -1, 0);
        }
        else if(direction == 2)
        {
            transform.localPosition = transform.localPosition + new Vector3(-1, 0, 0);
            transform.localPosition = transform.localPosition + new Vector3(-1, 0, 0);
        }

        else if(direction == 3)
        {
            transform.localPosition = transform.localPosition + new Vector3(1, 0, 0);
            transform.localPosition = transform.localPosition + new Vector3(1, 0, 0);
        }
    }
}
