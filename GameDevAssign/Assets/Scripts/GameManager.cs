using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // Spawn all players
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Vector2 spawnPoint = GenerateSpawnPoint();
            spawnPoints.Add(spawnPoint);

            players[i] = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
            players[i].GetComponent<Character>().playerNumber = i + 1;
        }

        // Initially assign one zombie
        AssignOneZombie();
    }

    void Update()
    {
        // You can call UpdateZombieCount() periodically if needed
    }

    // Assign one random player to be a zombie at the start of the game
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

    // Generate valid spawn points ensuring a safe distance between players
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

            // Ensure the spawn point is a safe distance from other players
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

    // Update the zombie count after any state changes
    public void UpdateZombieCount()
    {
        int currentZombieCount = 0;

        // Count zombies on the field
        foreach (GameObject player in players)
        {
            Character character = player.GetComponent<Character>();
            if (character.state == PlayerState.isZombie)
            {
                currentZombieCount++;
            }
        }

        // Update the global zombie count
        zombieCount = currentZombieCount;

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

    // Method to notify the GameManager when a state switch occurs
    public void NotifyStateChange()
    {
        UpdateZombieCount();
    }
}
