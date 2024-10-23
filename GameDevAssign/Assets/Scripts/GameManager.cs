using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private List<Vector2> spawnPoints = new List<Vector2>();
    private GameObject[] players;
    private Vector2 mapBounds;
    public float safeDistance = 5f; // Minimum safe distance between players

    public int numberOfPlayers;
    private int zombieCount;

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
            players[i].GetComponent<Character>().InitializeCharacter(i + 1, 60f);

            // Update UI for each player
            GameUIManager.Instance.UpdatePlayerInfo(i + 1, 60f);
        }

        // Initially assign one zombie
        AssignOneZombie();
    }

    // Assign one random player to be a zombie at the start of the game
    void AssignOneZombie()
    {
        int zombieIndex = Random.Range(0, numberOfPlayers);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i].GetComponent<Character>().state = i == zombieIndex ? PlayerState.isZombie : PlayerState.isCiv;
            players[i].GetComponent<Character>().UpdateState();
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
                Random.Range(-mapBounds.x / 2, mapBounds.x / 2),
                Random.Range(-mapBounds.y / 2, mapBounds.y / 2)
            );

            isValid = true;

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

        foreach (GameObject player in players)
        {
            if (player.GetComponent<Character>().state == PlayerState.isZombie)
            {
                currentZombieCount++;
            }
        }

        zombieCount = currentZombieCount;

        if (zombieCount == 0)
        {
            List<int> civilianIndices = new List<int>();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (players[i].GetComponent<Character>().state == PlayerState.isCiv)
                {
                    civilianIndices.Add(i);
                }
            }

            if (civilianIndices.Count > 0)
            {
                int newZombieIndex = civilianIndices[Random.Range(0, civilianIndices.Count)];
                players[newZombieIndex].GetComponent<Character>().state = PlayerState.isZombie;
                players[newZombieIndex].GetComponent<Character>().UpdateState();
            }
        }

        CheckForGameOver();
    }

    public void NotifyStateChange()
    {
        UpdateZombieCount();
    }

    void CheckForGameOver()
    {
        if (zombieCount == numberOfPlayers)
        {
            Debug.Log("Game Over! All players are zombies.");
        }
    }
}
