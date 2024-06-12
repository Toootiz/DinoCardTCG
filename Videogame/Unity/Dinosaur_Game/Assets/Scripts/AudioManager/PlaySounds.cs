using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnSelect : MonoBehaviour, ISelectHandler
{
    public AudioSource audioSource; // El Audio Source donde se reproducirá el sonido

    public void OnSelect(BaseEventData eventData)
    {
        // Reproduce el sonido cuando el elemento es seleccionado
        audioSource.Play();
    }
}
