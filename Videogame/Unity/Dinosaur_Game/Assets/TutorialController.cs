using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegúrate de incluir este namespace

public class TutorialController : MonoBehaviour
{
    public GameObject panel; // El panel del tutorial
    public Image tutorialImage; // La imagen del tutorial
    public TextMeshProUGUI tutorialText; // El texto del tutorial usando TextMeshPro
    public Button nextButton; // Botón siguiente
    public Button backButton; // Botón volver
    public Button closeButton; // Botón cerrar

    private int currentStep = 0; // El paso actual del tutorial
    private List<TutorialStep> tutorialSteps = new List<TutorialStep>(); // Lista de pasos del tutorial

    void Start()
    {
        LoadTutorialSteps(); // Cargar los pasos del tutorial
        panel.SetActive(true); // Muestra el panel al inicio del juego
        UpdateTutorialStep(); // Muestra el primer paso
        backButton.gameObject.SetActive(false); // Oculta el botón de volver al inicio
        closeButton.onClick.AddListener(ClosePanel); // Asigna la función al botón cerrar
        nextButton.onClick.AddListener(NextStep); // Asigna la función al botón siguiente
        backButton.onClick.AddListener(PreviousStep); // Asigna la función al botón volver
    }

    void LoadTutorialSteps()
    {
        // Añade tus pasos de tutorial aquí
        tutorialSteps.Add(new TutorialStep { image = Resources.Load<Sprite>("Tutu/1"), text = "Bienvenido al juego! Al iniciar se cargarán en la banca tus cartas del deck." });
        tutorialSteps.Add(new TutorialStep { image = Resources.Load<Sprite>("Tutu/1"), text = "Tienes ambar el cual esta en amarillo, la vida de tu base en verde, la base enemiga en rojo, en la esquina inferior derecha se muestra el turno actual y más abajo se muestra el turno actual y en la esquina superior izquierda se muestra lo que pasa." });
        tutorialSteps.Add(new TutorialStep { image = Resources.Load<Sprite>("Tutu/3"), text = " En cada turno tienes cuatro opciones, jugar una carta, atacar una carta, atacar la base enemiga o terminar turno. Hay una quinta opción pero depende de las cartas que haya en la zona de juego."});
        tutorialSteps.Add(new TutorialStep { image = Resources.Load<Sprite>("Tutu/4"), text = "Para poder jugar la carta y atacar se necesita tener la cantidad de ambar que indica el coste de la carta"});
        tutorialSteps.Add(new TutorialStep { image = Resources.Load<Sprite>("Tutu/5"), text = "En el luego hay cartas que aumentan características de otras cartas, esta es la quinta opción, puedes seleccionar la carta que quieres potenciar, y luego la carta que potencia"});
        tutorialSteps.Add(new TutorialStep { image = Resources.Load<Sprite>("Tutu/6"), text = "Esto gastara el ámbar de la carta con efecto y la destruirá, pero aumenta la característica"});
        tutorialSteps.Add(new TutorialStep { image = Resources.Load<Sprite>("Tutu/7"), text = "En el juego hay cartas que tienen efectos, estos efectos se aplican al atacar"});
        tutorialSteps.Add(new TutorialStep { image = Resources.Load<Sprite>("Tutu/7"), text = "Puedes ver las cartas con efectos, debido a una marca que les aparece y se queda hasta que el efecto termine"});
        tutorialSteps.Add(new TutorialStep { image = Resources.Load<Sprite>("Tutu/7"), text = "La energía va aumentando y se va acomulando a lo largo de la partida"});

        

        // Continúa añadiendo más pasos según sea necesario
    }

    void ClosePanel()
    {
        panel.SetActive(false); // Oculta el panel
    }

    void NextStep()
    {
        if (currentStep < tutorialSteps.Count - 1)
        {
            currentStep++;
            UpdateTutorialStep();
        }
        if (currentStep == tutorialSteps.Count - 1)
        {
            nextButton.gameObject.SetActive(false); // Oculta el botón de siguiente al final
        }
        backButton.gameObject.SetActive(true); // Muestra el botón de volver cuando avanzas
    }

    void PreviousStep()
    {
        if (currentStep > 0)
        {
            currentStep--;
            UpdateTutorialStep();
        }
        if (currentStep == 0)
        {
            backButton.gameObject.SetActive(false); // Oculta el botón de volver al inicio
        }
        nextButton.gameObject.SetActive(true); // Muestra el botón de siguiente si no es el último paso
    }

    void UpdateTutorialStep()
    {
        tutorialImage.sprite = tutorialSteps[currentStep].image;
        tutorialText.text = tutorialSteps[currentStep].text;
    }
}

[System.Serializable]
public class TutorialStep
{
    public Sprite image; // Imagen del paso del tutorial
    public string text; // Texto del paso del tutorial
}
