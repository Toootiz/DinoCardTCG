using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public void Restart() {
        SceneManager.LoadScene("MenuInicial");
    }

    public void MainMenu() {
        SceneManager.LoadScene("MenuInicial");
    }


    public void Score() {
        SceneManager.LoadScene("Score");
    }
}
