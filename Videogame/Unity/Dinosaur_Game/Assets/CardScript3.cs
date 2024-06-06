using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardScript3 : MonoBehaviour
{
    // Variables para guardar los datos que la carta recibe
    public int CardId, CardLife, CardAttack, CardCost, CardHabilidad;
    public string CardName;
    public Image CardArt;

    // Referencias a los componentes de la interfaz
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardLifeText;
    public TextMeshProUGUI cardAttackText;
    public TextMeshProUGUI cardCostText;
    public TextMeshProUGUI cardHabilidadText;

    void Start()
    {
        UpdateCardUI();
    }

    // Método para actualizar la interfaz de la carta
    public void UpdateCardUI()
    {
        if (cardNameText != null) cardNameText.text = CardName;
        if (cardLifeText != null) cardLifeText.text = CardLife.ToString();
        if (cardAttackText != null) cardAttackText.text = CardAttack.ToString();
        if (cardCostText != null) cardCostText.text = CardCost.ToString();
        if (cardHabilidadText != null) cardHabilidadText.text = CardHabilidad.ToString();
        // Asegúrate de que la imagen está configurada correctamente
        if (CardArt != null) CardArt.sprite = Resources.Load<Sprite>($"DinoImages/{CardId}");
    }
}
