using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectManager : MonoBehaviour
{
    public GameObject playerSelect; // Assign your player select UI here
    public GameObject mapSelect;    // Assign your map select UI here
                                    // Assign your map 2 button here
    private int activePlayers = 0;
    private Button numOfPlayers;

    void Start()
    {

    }



    public void BackToTitleScreen()
    {
        if (mapSelect.activeSelf)
        {

            SceneManager.LoadScene("TitleScreen");
        }
        else if (!mapSelect.activeSelf && playerSelect.activeSelf)
        {
            playerSelect.SetActive(false);
            mapSelect.SetActive(true);
        }
    }

    public void StartGame(int option)
    {

        if (option == 1)
        {
            // Logic for option 1
            GameData.PlayerCount = 3;  // Example: set player count for option 1
        }
        else if (option == 2)
        {
            // Logic for option 2
            GameData.PlayerCount = 4;  // Example: set player count for option 2
        }

        // Start the game (load the next scene)
        SceneManager.LoadScene("jom_Scene");
    }
}
