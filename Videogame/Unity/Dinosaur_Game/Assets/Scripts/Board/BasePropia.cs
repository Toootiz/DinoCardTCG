/*
Codigo que maneja la vida e interaccion con la base del jugador.
TAG del panel "Juego"
29/05/24
*/
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BasePropia : MonoBehaviour, IPointerClickHandler
{
    public int maxHealth = 40; // Salud máxima de la base.
    public int vidaActual; // Salud actual de la base.
    public TextMeshProUGUI vidaTexto; // Componente UI para mostrar la salud.
    public Slider healthBar; // Componente UI para mostrar la barra de salud visualmente. (De momento no se ha agregado)
    private GameManagement gameManagement; // Referencia al gestor del juego.

    void Start()
    {
        vidaActual = maxHealth; // Establece la salud actual a la máxima inicialmente.
        UpdateHealthDisplay(); // Actualiza la visualización de la salud.
        // Busca el objeto de gestión del juego por etiqueta.
        gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
    }

    // Aqui se hace el daño recibido por el enemigo.
    public void TakeDamage(int damage)
    {
        vidaActual -= damage; // Reduce la salud actual según el daño recibido.
        UpdateHealthDisplay(); // Actualiza la visualización de la salud.
        if (vidaActual <= 0) // Chequea si la salud ha llegado a cero o menos.
        {
            Debug.Log("La base del jugador ha sido destruida!");

            // Guardar el ID del jugador como 4 y la cantidad de turnos en PlayerPrefs
            PlayerPrefs.SetInt("LastWinningPlayerId", 4);
            PlayerPrefs.SetInt("TurnCount", gameManagement.JugadorContadorTurno);
            PlayerPrefs.Save();

            // Carga la escena de 'GameOver' cuando la base es destruida.
            SceneManager.LoadScene("GameOver");
        }
    }

    // Método para actualizar los elementos de UI que muestran la salud.
    public void UpdateHealthDisplay()
    {
        vidaTexto.text = "Vida: " + vidaActual; // Actualiza el texto de la salud.
        if (healthBar != null)
            healthBar.value = (float)vidaActual / maxHealth; // Actualiza la barra de salud.
    }

    // Método que se invoca cuando se hace clic en la base.
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Has seleccionado tu base.");
        // La lógica específica cuando se selecciona la base puede ser manejada por GameManagement.
    }
}
