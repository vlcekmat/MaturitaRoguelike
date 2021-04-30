using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script handles procedural generation of the room layout
public class RoomSpawner : MonoBehaviour
{
    public GameObject[] roomBank;
    public GameObject exitRoomPrefab;
    public GameObject gatePrefab;

    public List<Vector3> roomDatabase;
    public List<Vector3> gateDatabase;

    public int roomCount = 5;

    private Minimap minimap;


    Gate.Directions[] directions = { Gate.Directions.UP, Gate.Directions.DOWN, Gate.Directions.LEFT, Gate.Directions.RIGHT };

    private void Start()
    {   
        minimap = FindObjectOfType<Minimap>();
        roomCount = (FindObjectOfType<GameLogic>().currentLevel + 2) * 2; 
        roomDatabase.Add(transform.position);

        GenerateDungeon();
        minimap.ResetCam();
    }

    // initiates the generating process
    private void GenerateDungeon()
    {

        for (int i = 0; i < roomCount; i++)
        {
            // Choose random direction
            Gate.Directions direction = directions[UnityEngine.Random.Range(0, directions.Length)];

            GameObject roomToSpawn;

            // Choose random room
            if(i >= roomCount - 1)
            {
                roomToSpawn = exitRoomPrefab;
            }
            else
            {
                roomToSpawn = roomBank[UnityEngine.Random.Range(0, roomBank.Length)];
            }
            

            // Place a room and check if it was successful
            bool successful = PlaceRoom(direction, roomToSpawn);

            // If unsuccessful, try again
            if (!successful)
            {
                i--;
            }
        }
    }

    // spawns a room
    private Boolean PlaceRoom(Gate.Directions direction, GameObject roomToSpawn)
    {
        Vector3 roomShift = new Vector3(0f, 0f, 0f);
        switch (direction)
        {
            case Gate.Directions.UP:
                roomShift = new Vector3(0f, 10f, 0f);
                break;
            case Gate.Directions.DOWN:
                roomShift = new Vector3(0f, -10f, 0f);
                break;
            case Gate.Directions.LEFT:
                roomShift = new Vector3(-20f, 0f, 0f);
                break;
            case Gate.Directions.RIGHT:
                roomShift = new Vector3(20f, 0f, 0f);
                break;
        }

        if (!roomDatabase.Contains(transform.position + roomShift))
        {
            GameObject entranceGate = PlaceEntranceGate(direction);
            transform.position = transform.position + roomShift;
            Instantiate(roomToSpawn, transform.position, Quaternion.identity);
            roomDatabase.Add(transform.position);
            GameObject partnerGate = PlacePartnerGate(entranceGate, direction);

            switch (direction)
            {
                case Gate.Directions.UP:
                    minimap.CreatePath(0);
                    break;
                case Gate.Directions.DOWN:
                    minimap.CreatePath(1);
                    break;
                case Gate.Directions.LEFT:
                    minimap.CreatePath(2);
                    break;
                case Gate.Directions.RIGHT:
                    minimap.CreatePath(3);
                    break;
            }

            return true;
        }
        else
        {
            bool transportAdded = TryPlaceOnlyGate(direction, roomShift);
            if (transportAdded) 
            {   
                switch (direction)
                {
                    case Gate.Directions.UP:
                        minimap.CreatePath(0);
                        break;
                    case Gate.Directions.DOWN:
                        minimap.CreatePath(1);
                        break;
                    case Gate.Directions.LEFT:
                        minimap.CreatePath(2);
                        break;
                    case Gate.Directions.RIGHT:
                        minimap.CreatePath(3);
                        break;
                }
                return true; 
            }
            else { return false; }
        }
    }

    // if the spawner entered an already existing room, this function tries to add a gate to this existing room
    private Boolean TryPlaceOnlyGate(Gate.Directions direction, Vector3 roomShift)
    {
        transform.position = transform.position + roomShift;

        Gate.Directions partnerDirection = Gate.Directions.UP;
        switch (direction)
        {
            case Gate.Directions.UP:
                partnerDirection = Gate.Directions.DOWN;
                break;
            case Gate.Directions.DOWN:
                partnerDirection = Gate.Directions.UP;
                break;
            case Gate.Directions.LEFT:
                partnerDirection = Gate.Directions.RIGHT;
                break;
            case Gate.Directions.RIGHT:
                partnerDirection = Gate.Directions.LEFT;
                break;
        }

        Vector3 gateShift = new Vector3(0f, 0f, 0f);
        switch (partnerDirection)
        {
            case Gate.Directions.UP:
                gateShift = new Vector3(0.5f, 3.48f, 0f);
                break;
            case Gate.Directions.DOWN:
                gateShift = new Vector3(0.5f, -1.903f, 0f);
                break;
            case Gate.Directions.LEFT:
                gateShift = new Vector3(-5.812f, 0.56f, 0f);
                break;
            case Gate.Directions.RIGHT:
                gateShift = new Vector3(6.812f, 0.56f, 0f);
                break;
        }

        if (!gateDatabase.Contains(transform.position + gateShift))
        {
            transform.position = transform.position - roomShift;
            GameObject entranceGate = PlaceEntranceGate(direction);
            transform.position = transform.position + roomShift;
            GameObject partnerGate = PlacePartnerGate(entranceGate, direction);
            return true;
        }

        else
        {
            transform.position = transform.position - roomShift;
            return false;
        }
    }

    // spawns entrance gates according to the relative direction to the neighboring rooms
    private GameObject PlaceEntranceGate(Gate.Directions direction)
    {
        Vector3 gateShift = new Vector3(0f, 0f, 0f);

        switch (direction)
        {
            case Gate.Directions.UP:
                gateShift = new Vector3(0.5f, 3.48f, 0f);
                break;
            case Gate.Directions.DOWN:
                gateShift = new Vector3(0.5f, -1.903f, 0f);
                break;
            case Gate.Directions.LEFT:
                gateShift = new Vector3(-5.812f, 0.56f, 0f);
                break;
            case Gate.Directions.RIGHT:
                gateShift = new Vector3(6.812f, 0.56f, 0f);
                break;
        }

        GameObject entranceGate = Instantiate(gatePrefab, transform.position + gateShift, Quaternion.identity);
        gateDatabase.Add(transform.position + gateShift);
        entranceGate.GetComponent<Gate>().gateDirection = direction;
        return entranceGate;
    }

    // spawn another gate, that will be connected to the first one
    private GameObject PlacePartnerGate(GameObject entranceGate, Gate.Directions direction)
    {
        Gate.Directions partnerDirection = Gate.Directions.UP;
        switch (direction)
        {
            case Gate.Directions.UP:
                partnerDirection = Gate.Directions.DOWN;
                break;
            case Gate.Directions.DOWN:
                partnerDirection = Gate.Directions.UP;
                break;
            case Gate.Directions.LEFT:
                partnerDirection = Gate.Directions.RIGHT;
                break;
            case Gate.Directions.RIGHT:
                partnerDirection = Gate.Directions.LEFT;
                break;
        }

        Vector3 gateShift = new Vector3(0f, 0f, 0f);
        switch (partnerDirection)
        {
            case Gate.Directions.UP:
                gateShift = new Vector3(0.5f, 3.48f, 0f);
                break;
            case Gate.Directions.DOWN:
                gateShift = new Vector3(0.5f, -1.903f, 0f);
                break;
            case Gate.Directions.LEFT:
                gateShift = new Vector3(-5.812f, 0.56f, 0f);
                break;
            case Gate.Directions.RIGHT:
                gateShift = new Vector3(6.812f, 0.56f, 0f);
                break;
        }


        GameObject partnerGate = Instantiate(gatePrefab, transform.position + gateShift, Quaternion.identity);
        gateDatabase.Add(transform.position + gateShift);
        partnerGate.GetComponent<Gate>().gateDirection = partnerDirection;

        entranceGate.GetComponent<Gate>().partnerGate = partnerGate.GetComponent<Gate>();
        partnerGate.GetComponent<Gate>().partnerGate = entranceGate.GetComponent<Gate>();

        return partnerGate;
    }
}
