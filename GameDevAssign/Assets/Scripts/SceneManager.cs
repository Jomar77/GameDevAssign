using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectManager : MonoBehaviour
{
    public GameObject playerSelect;
    public GameObject mapSelect;
    public Image Panel;
    public Sprite anteMap;             // First sprite option
    public Sprite jomMap;
    public GameObject p4;

    //private int activePlayers = p4;
    //private Button numOfPlayers;

    void Start()
    {

    }



    public void BackToTitleScreen()
    {
        if (mapSelect.activeSelf)
        {

            SceneManager.LoadScene("Main Screen");
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

        if(Panel.sprite == anteMap)
        {
            SceneManager.LoadScene("gab_SampleScene");
        }
        if(Panel.sprite == jomMap)
        {
            SceneManager.LoadScene("jom_SampleScene");
        }
    }

    public void bgmap2()
    {
        Panel.sprite = anteMap;
    }

    public void bgmap1()
    {
        Panel.sprite = jomMap;
    }

    public void HidePrefab()
    {
        if (p4 != null)
        {
            p4.SetActive(false);
        }
    }

    // Method to show the prefab
    public void ShowPrefab()
    {
        if (p4 != null)
        {
            p4.SetActive(true);
        }
    }

}
