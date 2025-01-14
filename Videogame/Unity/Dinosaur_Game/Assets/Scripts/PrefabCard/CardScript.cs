using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int CardId, CardLife, CardAttack, CardCost, CardHabilidad, Cardvenenodmg, Cardquemadodmg, Cardsangradodmg, Cardmordidadmg, Cardcolatazodmg, Cardboostvida, Cardboostataquedmg, Cardboostcosto, Cardduracion;
    public string CardName, descripcion;
    public Image CardArt;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool canDrag = false;
    public bool isEnemyCard = false;
    private Vector3 initialPosition;
    private Transform originalParent;

    private bool isPlayed = false;
    public static CardScript selectedAttacker;
    private GameManagement gameManagement;
    private BaseEnemiga baseEnemiga;

    public TextMeshProUGUI LifeText;

    // Variables para efectos
    public int DuracionVeneno;
    public int DuracionQuemadura;
    public int DuracionSangrado;
    public int DañoVeneno;
    public int DañoQuemadura;
    public int DañoSangrado;

    // CanvasGroup para las imágenes de efectos
    public CanvasGroup venomEffect;
    public CanvasGroup burnEffect;
    public CanvasGroup bleedEffect;

    void Start()
    {
        gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
        baseEnemiga = GameObject.FindGameObjectWithTag("BaseEnemiga").GetComponent<BaseEnemiga>();
        rectTransform = GetComponent<RectTransform>();
        LifeText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        originalParent = transform.parent;

        // Inicializar CanvasGroup de los efectos
        venomEffect.alpha = 0;
        burnEffect.alpha = 0;
        bleedEffect.alpha = 0;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEnemyCard)
        {
            gameManagement.SituacionTexto.text = "No puedes mover cartas enemigas.";
            Debug.Log("No puedes mover cartas enemigas.");
            return;
        }

        if (!isPlayed && gameManagement.ambar >= CardCost)
        {
            canDrag = true;
            initialPosition = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canDrag = false;
            gameManagement.SituacionTexto.text = "¡No tienes suficiente energía!";
            Debug.Log("¡No tienes suficiente energía!");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            rectTransform.anchoredPosition += eventData.delta / 2;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        if (canDrag)
        {
            Transform newParent = eventData.pointerEnter != null ? eventData.pointerEnter.transform : originalParent;
            JuegoPanelScript panelScript = newParent.GetComponent<JuegoPanelScript>();

            // Verificar si el panel tiene espacio antes de mover la carta
            if (panelScript != null && panelScript.cards.Count > panelScript.maxCards)
            {
                gameManagement.SituacionTexto.text = "El panel ya tiene el máximo de cartas permitidas.";
                Debug.Log("El panel ya tiene el máximo de cartas permitidas.");
                rectTransform.anchoredPosition = initialPosition;
                transform.SetParent(originalParent);
                return;
            }

            if (!isEnemyCard && newParent.CompareTag("JuegoEnemigo"))
            {
                gameManagement.SituacionTexto.text = "No puedes colocar cartas del jugador en el panel de juego enemigo.";
                Debug.Log("No puedes colocar cartas del jugador en el panel de juego enemigo.");
                rectTransform.anchoredPosition = initialPosition;
                transform.SetParent(originalParent);
                return;
            }

            if ((isEnemyCard && newParent.CompareTag("JuegoEnemigo")) || (!isEnemyCard && newParent.CompareTag("Juego")))
            {
                if (gameManagement.SpendEnergy(CardCost))
                {
                    isPlayed = true;
                    if (originalParent != newParent)
                    {
                        JuegoPanelScript oldPanelScript = originalParent.GetComponent<JuegoPanelScript>();
                        if (oldPanelScript != null)
                        {
                            oldPanelScript.RemoveCard(gameObject);
                        }
                    }

                    // Mover la carta al nuevo panel
                    transform.SetParent(newParent, false);
                    initialPosition = rectTransform.anchoredPosition;
                    originalParent = newParent;

                    // Reducir el coste si es mayor a 6
                    if ((newParent.CompareTag("Juego") || newParent.CompareTag("JuegoEnemigo")) && CardCost > 6)
                    {
                        CardCost -= 4;
                        UpdateCostDisplay();
                    }

                    // Luego agregarla a la lista del nuevo panel si tiene JuegoPanelScript
                    if (panelScript != null && !panelScript.cards.Contains(gameObject))
                    {
                        panelScript.cards.Add(gameObject);
                    }

                    Debug.Log("La carta se movió correctamente.");
                }
                else
                {
                    rectTransform.anchoredPosition = initialPosition;
                    transform.SetParent(originalParent);
                }
            }
            else
            {
                rectTransform.anchoredPosition = initialPosition;
                transform.SetParent(originalParent);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isEnemyCard && selectedAttacker == null)
        {
            gameManagement.SituacionTexto.text = "No puedes seleccionar cartas del enemigo sin tener un atacante.";
            Debug.Log("No puedes seleccionar cartas del enemigo sin tener un atacante.");
            return;
        }

        if (selectedAttacker == this)
        {
            DeselectCard();
            return;
        }

        if (!isEnemyCard && transform.parent.CompareTag("Juego") && selectedAttacker == null)
        {
            SelectCard();
        }
        else if (isEnemyCard && transform.parent.CompareTag("JuegoEnemigo") && selectedAttacker != null)
        {
            selectedAttacker.AttackCard(this);
        }
        else if (!isEnemyCard && (Cardboostvida > 0 || Cardboostataquedmg > 0 || Cardboostcosto > 0) && selectedAttacker != null && selectedAttacker.transform.parent.CompareTag("Juego"))
        {
            // Verificar si hay suficiente energía para usar el boost
            if (gameManagement.ambar >= CardCost)
            {
                // Realizar boost en una carta aliada
                ApplyBoost(selectedAttacker);
                selectedAttacker.DeselectCard();
                Destroy(gameObject);
            }
            else
            {
                gameManagement.SituacionTexto.text = "No tienes suficiente energía para usar el boost.";
                Debug.Log("No tienes suficiente energía para usar el boost.");
            }
        }
    }

    private void SelectCard()
    {
        if (selectedAttacker != null)
        {
            selectedAttacker.DeselectCard();
        }
        selectedAttacker = this;
        canvasGroup.alpha = 0.6f;
        gameManagement.SituacionTexto.text = $"Carta {CardName} seleccionada como atacante.";
        Debug.Log($"Carta {CardName} seleccionada como atacante.");
    }

    public void DeselectCard()
    {
        if (selectedAttacker == this)
        {
            selectedAttacker = null;
            canvasGroup.alpha = 1.0f;
            Debug.Log($"Carta {CardName} ha sido deseleccionada.");
        }
    }

    public void AttackCard(CardScript target)
    {
        if (target != null)
        {
            int attackCost = this.CardCost > 6 ? 2 : this.CardCost;
            if (gameManagement.SpendEnergy(attackCost))
            {
                int totalAttack = this.CardAttack + this.Cardmordidadmg + this.Cardcolatazodmg;
                target.CardLife -= totalAttack;
                target.UpdateLifeDisplay();
                gameManagement.SituacionTexto.text = $"Atacando a {target.CardName} con {CardName}. Vida restante: {target.CardLife}";
                Debug.Log($"Atacando a {target.CardName} con {CardName}. Vida restante: {target.CardLife}");

                // Aplicar efectos
                if (Cardvenenodmg > 0)
                {
                    target.ApplyEffect("Veneno", Cardvenenodmg, Cardduracion);
                }
                if (Cardquemadodmg > 0)
                {
                    target.ApplyEffect("Quemadura", Cardquemadodmg, Cardduracion);
                }
                if (Cardsangradodmg > 0)
                {
                    target.ApplyEffect("Sangrado", Cardsangradodmg, Cardduracion);
                }

                if (target.CardLife <= 0)
                {
                    gameManagement.SituacionTexto.text = $"{target.CardName} ha sido destruida.";
                    Debug.Log($"{target.CardName} ha sido destruida.");
                    JuegoEnemigoPanelScript enemyPanel = GameObject.FindGameObjectWithTag("JuegoEnemigo").GetComponent<JuegoEnemigoPanelScript>();
                    if (enemyPanel != null && target.isEnemyCard)
                    {
                        enemyPanel.RemoveEnemyCard(target.gameObject);
                    }
                    Destroy(target.gameObject);
                }

                DeselectCard();
            }
            else
            {
                gameManagement.SituacionTexto.text = "No hay suficiente ámbar para atacar.";
                Debug.Log("No hay suficiente ámbar para atacar.");
            }
        }
        else
        {
            gameManagement.SituacionTexto.text = "Objetivo no válido o ámbar insuficiente.";
            Debug.Log("Objetivo no válido o ámbar insuficiente.");
        }
    }

    public void TakeDamage(int damage)
    {
        CardLife -= damage;
        UpdateLifeDisplay();
    }

    private void UpdateLifeDisplay()
    {
        if (LifeText != null)
            LifeText.text = CardLife.ToString();
        if (CardLife <= 0)
            Destroy(gameObject);
    }

    private void UpdateAttackDisplay()
    {
        TextMeshProUGUI attackText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        if (attackText != null)
            attackText.text = CardAttack.ToString();
        if (CardAttack <= 0)
            Destroy(gameObject);
    }

    public void UpdateCostDisplay()
    {
        TextMeshProUGUI costText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        if (costText != null)
            costText.text = CardCost.ToString();
        if (CardCost <= 0)
            CardCost = 0; // Ajuste para que el coste no sea negativo
    }

    public void OnClic()
    {
        // Este método está vacío, puede ser eliminado o utilizado para otra función.
    }

    // Método para usar habilidades de la carta
    public void UseHabilidad()
    {
        if (gameManagement.SpendEnergy(CardHabilidad))
        {
            gameManagement.SituacionTexto.text = $"{CardName} ha usado su habilidad.";
            // Implementar la lógica de la habilidad aquí
            Debug.Log($"{CardName} ha usado su habilidad.");
            // Ejemplo de implementación de habilidades
            if (Cardvenenodmg > 0)
            {
                // Aplicar veneno a la carta objetivo
                // target.TakeDamage(Cardvenenodmg);
            }
            if (Cardquemadodmg > 0)
            {
                // Aplicar quemadura a la carta objetivo
                // target.TakeDamage(Cardquemadodmg);
            }
            // Y así sucesivamente para otras habilidades
        }
        else
        {
            gameManagement.SituacionTexto.text = "No hay suficiente ámbar para usar la habilidad.";
            Debug.Log("No hay suficiente ámbar para usar la habilidad.");
        }
    }

    public void ApplyEffect(string effectType, int damage, int duration)
    {
        switch (effectType)
        {
            case "Veneno":
                DañoVeneno = damage;
                DuracionVeneno = duration;
                venomEffect.alpha = 1;
                break;
            case "Quemadura":
                DañoQuemadura = damage;
                DuracionQuemadura = duration;
                burnEffect.alpha = 1;
                break;
            case "Sangrado":
                DañoSangrado = damage;
                DuracionSangrado = duration;
                bleedEffect.alpha = 1;
                break;
        }
        gameManagement.SituacionTexto.text = $"{CardName} ha recibido el efecto de {effectType} por {duration} turnos.";
        Debug.Log($"{CardName} ha recibido el efecto de {effectType} por {duration} turnos.");
    }

    public void ApplyEffectDamage()
    {
        if (DuracionVeneno > 0)
        {
            CardLife -= DañoVeneno;
            DuracionVeneno--;
            if (DuracionVeneno == 0)
            {
                venomEffect.alpha = 0;
            }
        }
        if (DuracionQuemadura > 0)
        {
            CardLife -= DañoQuemadura;
            DuracionQuemadura--;
            if (DuracionQuemadura == 0)
            {
                burnEffect.alpha = 0;
            }
        }
        if (DuracionSangrado > 0)
        {
            CardLife -= DañoSangrado;
            DuracionSangrado--;
            if (DuracionSangrado == 0)
            {
                bleedEffect.alpha = 0;
            }
        }
        UpdateLifeDisplay();
    }

    public void ApplyBoost(CardScript target)
    {
        if (Cardboostvida > 0)
        {
            target.CardLife += Cardboostvida;
            target.UpdateLifeDisplay();
            gameManagement.SituacionTexto.text = $"{CardName} ha aumentado la vida de {target.CardName} en {Cardboostvida} puntos.";
            Debug.Log($"{CardName} ha aumentado la vida de {target.CardName} en {Cardboostvida} puntos.");
        }
        if (Cardboostataquedmg > 0)
        {
            target.CardAttack += Cardboostataquedmg;
            target.UpdateAttackDisplay();
            gameManagement.SituacionTexto.text = $"{CardName} ha aumentado el ataque de {target.CardName} en {Cardboostataquedmg} puntos.";
            Debug.Log($"{CardName} ha aumentado el ataque de {target.CardName} en {Cardboostataquedmg} puntos.");
        }
        if (Cardboostcosto > 0)
        {
            target.CardCost = Mathf.Max(0, target.CardCost - Cardboostcosto);
            target.UpdateCostDisplay();
            gameManagement.SituacionTexto.text = $"{CardName} ha reducido el coste de {target.CardName} en {Cardboostcosto} puntos.";
            Debug.Log($"{CardName} ha reducido el coste de {target.CardName} en {Cardboostcosto} puntos.");
        }

        // Gastar energía después de aplicar el efecto
        gameManagement.SpendEnergy(CardCost);

        // Destruir la carta de boost después de aplicar el efecto
        Destroy(gameObject);
    }
}
