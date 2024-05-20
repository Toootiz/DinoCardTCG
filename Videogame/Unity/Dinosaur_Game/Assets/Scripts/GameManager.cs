using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text playerNameText;
    public Text playerScoreText;
    public Text gameLevelText;

    // Método para inicializar los datos del jugador y de la partida en la pantalla de juego
    public void UpdateGameScreen(string playerName, int playerScore, int gameLevel)
    {
        playerNameText.text = "Player: " + playerName;
        playerScoreText.text = "Score: " + playerScore.ToString();
        gameLevelText.text = "Level: " + gameLevel.ToString();
    }
}
