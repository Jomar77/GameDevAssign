using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameUIManager : MonoBehaviour
{

    public GameObject playerPanelPrefab;
    public Transform panelParent;
    private TMP_Text playerInfoNum;
    private TMP_Text playerInfoTime;
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


        playerInfoNum = newPanel.transform.Find("PlayerNum").GetComponent<TMP_Text>();

        if (playerInfoNum == null)
        {
            Debug.LogError("PlayerNum Text component is missing!");
            return;
        }
        playerInfoNum.text = "Player " + player.playerNumber;

        playerInfoTime = newPanel.transform.Find("TimeLeft").GetComponent<TMP_Text>();

        if (playerInfoTime == null)
        {
            Debug.LogError("TimeLeft Text component is missing!");
            return;
        }
        playerInfoTime.text = "Time Left: " + player.remainingTime.ToString("F1");


        playerPanels[player.playerNumber] = newPanel;

        AdjustPanelScale(newPanel);

    }

    public void UpdatePlayerInfo(int playerNumber, float remainingTime)
    {
        if (playerPanels.ContainsKey(playerNumber))
        {
            playerInfoNum = playerPanels[playerNumber].transform.Find("PlayerNum").GetComponent<TMP_Text>();
            playerInfoNum.text = "Player " + playerNumber;


            playerInfoTime = playerPanels[playerNumber].transform.Find("TimeLeft").GetComponent<TMP_Text>();
            playerInfoTime.text = "Time: " + Mathf.Max(0, remainingTime).ToString("F1");

            AdjustPanelScale(playerPanels[playerNumber]);
        }
    }


    private void AdjustPanelScale(GameObject panel)
    {
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        float parentWidth = panelParent.GetComponent<RectTransform>().rect.width;

        // Calculate the total width of the player panel including its padding
        float totalWidth = 0f;

        foreach (RectTransform child in panelRect)
        {
            totalWidth += child.rect.width + ((HorizontalLayoutGroup)panelParent.GetComponent<LayoutGroup>()).spacing;
        }

        // Calculate the scale factor to fit the parent width
        float scaleFactor = 1f;

        if (totalWidth > parentWidth)
        {
            scaleFactor = parentWidth / totalWidth;
        }

        // Apply the scale to the panel and its children
        panel.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

        // Optionally, you can also scale the individual child elements if needed
        foreach (RectTransform child in panelRect)
        {
            child.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }
    }
}

