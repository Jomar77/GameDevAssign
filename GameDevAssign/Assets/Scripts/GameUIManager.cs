using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    public GameObject playerPanelPrefab;
    public Transform panelParent;
    private Text playerInfoNum;
    private Text playerInfoTime;
    private Dictionary<int, GameObject> playerPanels = new Dictionary<int, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePlayerPanel(int playerNumber, float initialTime)
    {
        GameObject newPanel = Instantiate(playerPanelPrefab, panelParent);

        playerInfoNum = newPanel.transform.Find("PlayerNum").GetComponent<Text>();
        playerInfoNum.text = "Player " + playerNumber;

        playerInfoTime = newPanel.transform.Find("TimeLeft").GetComponent<Text>();
        playerInfoTime.text = "Time Left: " + initialTime.ToString("F1");


        playerPanels[playerNumber] = newPanel;
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

