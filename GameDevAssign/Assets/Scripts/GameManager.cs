using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private List<Vector2> spawnPoints = new List<Vector2>();
    private GameObject[] players;
    public Vector2 mapBounds = new Vector2(1920f, 1800f); // Map dimensions
    public float safeDistance = 200f; // Minimum safe distance between players

    public int numberOfPlayers;

    void Start()
    {
        players = new GameObject[numberOfPlayers];

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Vector2 spawnPoint = GenerateSpawnPoint();
            spawnPoints.Add(spawnPoint);

            players[i] = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
            players[i].GetComponent<Character>().playerNumber = i + 1;
        }
    }

    void Update()
    {

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




}
