using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BasePropia : MonoBehaviour, IPointerClickHandler
{
    public int maxHealth = 100;
    public int currentHealth;
    public TextMeshProUGUI healthText;
    public Slider healthBar; 
    private GameManagement gameManagement;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
        gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthDisplay();
        if (currentHealth <= 0)
        {
            Debug.Log("La base del jugador ha sido destruida!");
            // Puedes agregar lo que sucede cuando la base es destruida aquí.
            SceneManager.LoadScene("GameOver");
        }
    }

    public void UpdateHealthDisplay()
    {
        healthText.text = "Vida: " + currentHealth;
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Has seleccionado tu base.");
        // El ataque a la base del jugador se maneja en GameManagement.
    }
}
