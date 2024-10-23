using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    public Text[] playerInfoTexts;  // Text fields for each player (assign in Inspector)

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

    // Method to update the player's UI
    public void UpdatePlayerInfo(int playerNumber, float remainingTime)
    {
        if (playerNumber >= 1 && playerNumber <= playerInfoTexts.Length)
        {
            // Update the relevant text field
            playerInfoTexts[playerNumber - 1].text = "Player " + playerNumber + " | Time: " + Mathf.Max(0, remainingTime).ToString("F1");
        }
    }
}
