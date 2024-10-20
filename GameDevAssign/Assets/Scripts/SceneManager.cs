using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public GameObject playerSelect; // Assign your player select UI here
    public GameObject mapSelect;    // Assign your map select UI here
    public Button[] playerButtons;   // Assign your player control scheme buttons here
    public Button map1Button;        // Assign your map 1 button here
    public Button map2Button;        // Assign your map 2 button here
    private int activePlayers = 0;

    void Start()
    {
        // Set initial UI state
        ShowPlayerSelect();

        // Map button listeners
        map1Button.onClick.AddListener(() => LoadMap("Map1"));
        map2Button.onClick.AddListener(() => LoadMap("Map2"));

        // Player button listeners
        foreach (Button button in playerButtons)
        {
            button.onClick.AddListener(() => SelectPlayer(button));
        }
    }

    void ShowPlayerSelect()
    {
        playerSelect.SetActive(true);
        mapSelect.SetActive(false);
    }
    public void LoadMap(string mapName)
    {
        // Store the selected map or perform other logic
        Debug.Log("Selected Map: " + mapName);

        // Load the game scene (assuming you have a scene named "GameScene")
        SceneManager.LoadScene("GameScene");
    }

    void SelectPlayer(Button button)
    {
        if (button.interactable)
        {
            button.interactable = false;
            activePlayers++;

            // Limit the number of active players to 3
            if (activePlayers > 3)
            {
                DeactivatePlayer();
            }
        }
        else
        {
            button.interactable = true;
            activePlayers--;
        }
    }

    void DeactivatePlayer()
    {
        foreach (Button button in playerButtons)
        {
            if (!button.interactable)
            {
                button.interactable = true;
                activePlayers--;
                break; // Exit after deactivating one button
            }
        }
    }

    public void BackToPlayerSelect()
    {
        ShowPlayerSelect();
    }

    public void BackToTitleScreen()
    {
        if (mapSelect.activeSelf)
        {

            SceneManager.LoadScene("TitleScreen");
        }
        else if( !mapSelect.activeSelf && playerSelect.activeSelf)
        {
            playerSelect.SetActive(false);
            mapSelect.SetActive(true);
        }
    }
}
