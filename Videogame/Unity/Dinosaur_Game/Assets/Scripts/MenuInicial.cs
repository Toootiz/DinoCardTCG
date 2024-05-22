using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Board");
    }

    public void Salir(){
        Debug.Log("Salir");
        Application.Quit();
    }

    public void Deck()
    {
        SceneManager.LoadScene("Deck");

    }
}
