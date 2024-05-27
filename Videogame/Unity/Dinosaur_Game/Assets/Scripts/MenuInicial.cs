using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuInicial : MonoBehaviour
{

    
    public AudioMixer audioMixer;


    public void Jugar()
    {
        SceneManager.LoadScene("Board");
    }

    public void Salir(){
        Debug.Log("Salir");
        SceneManager.LoadScene("Prueba");
        //Application.Quit();
    }

    public void Deck()
    {
        SceneManager.LoadScene("Deck");

    }

    
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }



}
