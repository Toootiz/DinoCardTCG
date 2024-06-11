using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class BaseEnemiga : MonoBehaviour, IPointerClickHandler
{
    public int maxHealth = 40;
    public int currentHealth;
    public TextMeshProUGUI healthText;
    public Slider healthBar; 
    private GameManagement gameManagement;
    private Transform enemyGamePanel;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
        gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
        enemyGamePanel = GameObject.FindGameObjectWithTag("JuegoEnemigo").transform;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthDisplay();
        if (currentHealth <= 0)
        {
            Debug.Log("La base enemiga ha sido destruida!");
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
        Debug.Log("Has seleccionado la base enemiga");
        
        // Verifica si hay una carta seleccionada como atacante
        if (CardScript.selectedAttacker != null)
        {
            // Verifica si hay cartas en el panel de JuegoEnemigo
            if (enemyGamePanel.childCount > 0)
            {
                Debug.Log("No se puede atacar debido a que el enemigo aún tiene cartas.");
                return;
            }
            
            // Verifica si el contador de turnos del jugador es menor a 5
            if (gameManagement.JugadorContadorTurno < 2)
            {
                Debug.Log("No puedes atacar antes del turno 5.");
                return;
            }

            CardScript attacker = CardScript.selectedAttacker;
            
            if (gameManagement.ambar >= attacker.CardCost)
            {
                if (gameManagement.SpendEnergy(attacker.CardCost))
                {
                    TakeDamage(attacker.CardAttack);
                    Debug.Log($"Atacando la base enemiga con {attacker.CardName} por {attacker.CardAttack} de daño.");
                    attacker.DeselectCard();
                }
                else
                {
                    Debug.Log("No hay suficiente ámbar para atacar la base enemiga.");
                }
            }
            else
            {
                Debug.Log("Ámbar insuficiente para atacar la base enemiga.");
            }
        }
        else
        {
            Debug.Log("No hay carta atacante seleccionada.");
        }
    }
}
