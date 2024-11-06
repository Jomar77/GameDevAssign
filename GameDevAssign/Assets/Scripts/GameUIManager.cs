using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameUIManager : MonoBehaviour
{

    public GameObject playerPanelPrefab;
    public Transform panelParent;
    public GameObject gameOverScreen;
    private TMP_Text playerInfoNum;
    private TMP_Text playerInfoTime;


    public GameObject leaderboardItemPrefab;
    public Transform leaderboardPanel;
    private Dictionary<int, GameObject> playerPanels = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> leaderboardPanels = new Dictionary<int, GameObject>();

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


        set
        {

        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowGameOverScreen()
    {
        DisplaySortedLeaderboard();
        gameOverScreen.SetActive(true);
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

      

    }

    public GameObject pauseMenu;

    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // If the game is paused, unpause it
                ResumeGame();
            }
            else
            {
                // If the game is not paused, pause it
                PauseGameFunction();
            }
        }
    }

    // Function to pause the game
    void PauseGameFunction()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game (stops time)
        pauseMenu.SetActive(true); // Show the pause menu
    }

    // Function to resume the game
    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game (restores time)
        pauseMenu.SetActive(false); // Hide the pause menu
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

        float totalWidth = 0f;

        foreach (RectTransform child in panelRect)
        {
            totalWidth += child.rect.width + ((HorizontalLayoutGroup)panelParent.GetComponent<LayoutGroup>()).spacing;
        }

        float scaleFactor = 1f;

        if (totalWidth > parentWidth)
        {
            scaleFactor = parentWidth / totalWidth;
        }

        panel.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

        foreach (RectTransform child in panelRect)
        {
            child.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }
    }

    public void AddLeaderboardEntry(Character character)
    {
        int playerNumber = character.playerNumber;
        float finalTime = character.remainingTime;

        GameObject leaderboardEntry = Instantiate(leaderboardItemPrefab, leaderboardPanel);

        playerInfoNum = leaderboardEntry.transform.Find("Player Num").GetComponent<TMP_Text>();
        playerInfoNum.text = "Player " + playerNumber;


        playerInfoTime = leaderboardEntry.transform.Find("Player Time").GetComponent<TMP_Text>();
        playerInfoTime.text = "Time: " + Mathf.Max(0, finalTime).ToString("F1");



        leaderboardPanels[character.playerNumber] = leaderboardEntry;


    }

    public void UpdateleaderBoardInfo(int playerNumber, float remainingTime)
    {
        if (leaderboardPanels.ContainsKey(playerNumber))
        {
            playerInfoNum = leaderboardPanels[playerNumber].transform.Find("Player Num").GetComponent<TMP_Text>();
            playerInfoNum.text = "Player " + playerNumber;

            playerInfoTime = leaderboardPanels[playerNumber].transform.Find("Player Time").GetComponent<TMP_Text>();
            playerInfoTime.text = "Time: " + Mathf.Max(0, remainingTime).ToString("F1");

        }
    }

    public void DisplaySortedLeaderboard()
    {

        foreach (var character in GameManager.Instance.playerCharacters)
        {
            // Check if the character's playerNumber matches a key in the leaderboardPanels dictionary
            if (leaderboardPanels.ContainsKey(character.playerNumber))
            {
                leaderboardPanels[character.playerNumber].transform.Find("Player Time").GetComponent<TMP_Text>().text = character.remainingTime.ToString("F2");

            }
        }

    }

    public void returnMainScene()
    {
        
        SceneManager.LoadScene("Main Screen");
    }



}

