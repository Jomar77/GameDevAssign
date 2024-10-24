using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private List<Vector3> spawnPoints = new List<Vector3>();
    private GameObject[] players;
    private Vector2 mapBounds;

    public float safeDistance = 5f;
    Vector3 cameraPosition;
    public int numberOfPlayers;

    private Camera mainCamera;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mainCamera = Camera.main;
    }
    void Start()
    {
        mainCamera = Camera.main;
        cameraPosition = mainCamera.transform.position;

        players = new GameObject[numberOfPlayers];

        Camera cam = Camera.main;
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;
        mapBounds = new Vector2(cameraWidth, cameraHeight);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Vector3 spawnPoint = GenerateSpawnPoint();
            spawnPoints.Add(spawnPoint);

            players[i] = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
            players[i].GetComponent<Character>().InitializeCharacter(i + 1, 60f);

            GameUIManager.Instance.CreatePlayerPanel(players[i].GetComponent<Character>());
        }

        AssignOneZombie();
    }

    void AssignOneZombie()
    {
        int zombieIndex = Random.Range(0, numberOfPlayers);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i].GetComponent<Character>().state = i == zombieIndex ? PlayerState.isZombie : PlayerState.isCiv;
            players[i].GetComponent<Character>().UpdateState();
        }
    }

    Vector3 GenerateSpawnPoint()
    {
        Vector3 spawnPoint;
        bool isValid;

        do
        {
            spawnPoint = new Vector3(
                Random.Range(-mapBounds.x / 2, mapBounds.x / 2),
                Random.Range(-mapBounds.y / 2, mapBounds.y / 2),
                cameraPosition.z + 1f
            );

            isValid = true;

            foreach (Vector3 existingPoint in spawnPoints)
            {
                if (Vector3.Distance(spawnPoint, existingPoint) < safeDistance)
                {
                    isValid = false;
                    break;
                }
            }

        } while (!isValid);

        return spawnPoint;
    }

    public void StartZoom(float targetSize, float duration, System.Action onZoomComplete = null)
    {
        StartCoroutine(SmoothZoom(targetSize, duration, onZoomComplete));
    }

    private IEnumerator SmoothZoom(float targetSize, float duration, System.Action onZoomComplete = null)
    {
        float startSize = mainCamera.orthographicSize;
        float time = 0f;

        while (time < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetSize;

        onZoomComplete?.Invoke();
    }

    public void ZoomOutAfterDelay(float zoomOutSize, float delay, float duration)
    {
        StartCoroutine(ZoomOutDelayCoroutine(zoomOutSize, delay, duration));
    }

    private IEnumerator ZoomOutDelayCoroutine(float zoomOutSize, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);  // Wait for the specified delay
        StartZoom(zoomOutSize, duration);        // Start zoom out effect
    }
}
