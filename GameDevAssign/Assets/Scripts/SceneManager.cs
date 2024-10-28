using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectManager : MonoBehaviour
{
    public GameObject playerSelect; 
    public GameObject mapSelect;    
                                 
    //private int activePlayers = 0;
    //private Button numOfPlayers;

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
            GameData.PlayerCount = 3;
        }
        else if (option == 2)
        {
            GameData.PlayerCount = 4;  
        }

        SceneManager.LoadScene("jom_Scene");
    }
}
