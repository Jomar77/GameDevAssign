using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette;
    public GameObject playerPrefab;
    private List<Vector3> spawnPoints = new List<Vector3>();
    private GameObject[] players;
    private Vector2 mapBounds;

    private Vector3 initialCameraPosition;
    private float initialCameraSize;

    public List<GameObject> obstaclePrefabs;

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

        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.Override(0);
        }

        mainCamera = Camera.main;
    }
    void Start()
    {

        mainCamera = Camera.main;
        initialCameraPosition = mainCamera.transform.position;
        initialCameraSize = mainCamera.orthographicSize;
        numberOfPlayers = GameData.PlayerCount;

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
        SpawnObstacles(20);

    }

    // New method to spawn obstacles randomly
    public void SpawnObstacles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Randomly select an obstacle prefab from the list
            GameObject prefabToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
            Vector3 spawnPoint = GenerateSpawnPoint(); // Generate a random spawn point
            Instantiate(prefabToSpawn, spawnPoint, Quaternion.identity); // Instantiate the prefab at the spawn point
        }
    }

    public void EnableVignette(float intensity)
    {
        if (vignette != null)
        {
            vignette.intensity.value = intensity;
        }
    }
    public void DisableVignette()
    {
        if (vignette != null)
        {
            vignette.intensity.value = 0;
        }
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
                -7f
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

    public void StartCollisionZoom(Vector3 collisionPoint, float targetSize, float duration,float delay, System.Action onComplete)
    {
        foreach (var player in players)
        {
            player.GetComponent<Character>().isClampingEnabled = false;
        }

        StartCoroutine(CollisionZoomCoroutine(collisionPoint, targetSize, duration, delay,() =>
        {
            // Re-enable clamping after zoom completes
            foreach (var player in players)
            {
                player.GetComponent<Character>().isClampingEnabled = true;
            }

            // Reset camera position and size
            mainCamera.transform.position = initialCameraPosition;
            mainCamera.orthographicSize = initialCameraSize;
        }));
    }

    private IEnumerator CollisionZoomCoroutine(Vector3 targetPosition, float targetSize, float duration, float delay, System.Action onComplete)
    {
        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;
        float time = 0f;

        // Move and zoom in
        while (time < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;

        // Wait for the delay
        yield return new WaitForSeconds(delay);

        // Reset the camera back to its initial position and size
        time = 0f;
        while (time < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(targetPosition, initialCameraPosition, time / duration);
            mainCamera.orthographicSize = Mathf.Lerp(targetSize, initialCameraSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = initialCameraPosition;
        mainCamera.orthographicSize = initialCameraSize;

        onComplete?.Invoke();
    }


}
