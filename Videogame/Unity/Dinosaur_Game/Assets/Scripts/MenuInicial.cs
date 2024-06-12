using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MenuInicial : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TextMeshProUGUI errorMessage; // Usar TextMeshProUGUI en lugar de Text


    void Start()
    {
        int userId = PlayerPrefs.GetInt("userId", 0);
        Debug.Log(userId);
    }

    public void Jugar()
    {
        int deckId = PlayerPrefs.GetInt("SelectedDeckId", 0);
        if (deckId != 0)
        {
            SceneManager.LoadScene("Board");
        }
        else
        {
            ShowErrorMessage("Crea o selecciona un deck");
        }
    }

    public void Salir()
    {
        Debug.Log("Salir");
        SceneManager.LoadScene("Login");
        //Application.Quit();
    }

    public void Deck()
    {
        SceneManager.LoadScene("Deck");
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    private void ShowErrorMessage(string message)
    {
        Debug.Log("Selecciona un deck");
    }
}
