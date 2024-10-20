using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private List<Vector2> spawnPoints = new List<Vector2>();
    private GameObject[] players;
    public Vector2 mapBounds;
    public float safeDistance = 5f; // Minimum safe distance between players

    public int numberOfPlayers;
    private int zombieCount; // Keep track of the number of zombies


    void Start()
    {
        players = new GameObject[numberOfPlayers];

        Camera cam = Camera.main;
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;
        mapBounds = new Vector2(cameraWidth, cameraHeight);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Vector2 spawnPoint = GenerateSpawnPoint();
            spawnPoints.Add(spawnPoint);

            players[i] = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
            players[i].GetComponent<Character>().playerNumber = i + 1;
        }

        AssignOneZombie();
    }

    void Update()
    {

    }


    void AssignOneZombie()
    {
        int zombieIndex = Random.Range(0, numberOfPlayers);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (i == zombieIndex)
            {
                players[i].GetComponent<Character>().state = PlayerState.isZombie;
            }
            else
            {
                players[i].GetComponent<Character>().state = PlayerState.isCiv;
            }
        }

        zombieCount = 1;
    }

    Vector2 GenerateSpawnPoint()
    {
        Vector2 spawnPoint;
        bool isValid;

        do
        {
            spawnPoint = new Vector2(
                Random.Range(-mapBounds.x / 2, mapBounds.x / 2), // X within map bounds
                Random.Range(-mapBounds.y / 2, mapBounds.y / 2)  // Y within map bounds
            );

            isValid = true;

            // Check distance from existing spawn points
            foreach (Vector2 existingPoint in spawnPoints)
            {
                if (Vector2.Distance(spawnPoint, existingPoint) < safeDistance)
                {
                    isValid = false;
                    break;
                }
            }

        } while (!isValid);

        return spawnPoint;
    }


    public void UpdateZombieCount()
    {
        int zombieCount = 0;

        // Loop through all players and count the zombies
        foreach (GameObject player in players)
        {
            Character character = player.GetComponent<Character>();
            if (character.state == PlayerState.isZombie)
            {
                zombieCount++;
            }
        }

        // Ensure there's always at least one zombie
        if (zombieCount == 0)
        {
            // Assign a random civilian as the new zombie
            int newZombieIndex = Random.Range(0, numberOfPlayers);
            players[newZombieIndex].GetComponent<Character>().state = PlayerState.isZombie;
            players[newZombieIndex].GetComponent<Character>().UpdateState();
            Debug.Log("New zombie assigned!");
        }
    }






}
