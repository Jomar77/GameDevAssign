using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour
{

    public GameObject playerPanelPrefab;
    public Transform panelParent;
    private Text playerInfoNum;
    private Text playerInfoTime;
    private Dictionary<int, GameObject> playerPanels = new Dictionary<int, GameObject>();

    private static GameUIManager _instance;

    public static GameUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameUIManager>();

                if (_instance == null)
                {
                    Debug.LogError("There is no GameUIManager instance in the scene.");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Ensure there is only one instance of the GameUIManager
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Destroy the extra instance if it exists
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want it to persist across scenes
        }
    }


    public void CreatePlayerPanel(Character player)
    {
        GameObject newPanel = Instantiate(playerPanelPrefab, panelParent);

        if (player == null)
        {
            Debug.LogError("Player is null!");
            return;
        }


        playerInfoNum = newPanel.transform.Find("PlayerNum").GetComponent<Text>();

        if (playerInfoNum == null)
        {
            Debug.LogError("PlayerNum Text component is missing!");
            return;
        }
        playerInfoNum.text = "Player " + player.playerNumber;

        playerInfoTime = newPanel.transform.Find("TimeLeft").GetComponent<Text>();

        if (playerInfoTime == null)
        {
            Debug.LogError("TimeLeft Text component is missing!");
            return;
        }
        playerInfoTime.text = "Time Left: " + player.remainingTime.ToString("F1");


        playerPanels[player.playerNumber] = newPanel;
    }

    public void UpdatePlayerInfo(int playerNumber, float remainingTime)
    {
        if (playerPanels.ContainsKey(playerNumber))
        {
            playerInfoNum = playerPanels[playerNumber].transform.Find("PlayerNum").GetComponent<Text>();
            playerInfoNum.text = "Player " + playerNumber;


            playerInfoTime = playerPanels[playerNumber].transform.Find("TimeLeft").GetComponent<Text>();
            playerInfoTime.text = "Time: " + Mathf.Max(0, remainingTime).ToString("F1");
        }
    }
}

