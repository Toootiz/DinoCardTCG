using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public Button registerButton;
    public Button loginButton;
    public TMP_Text Estado;
    public CanvasGroup estadoCanvasGroup;

    void Start()
    {
        registerButton.onClick.AddListener(Register);
        loginButton.onClick.AddListener(Login);
        estadoCanvasGroup.alpha = 0;
    }

    void Register()
    {
        string nombre = usernameInputField.text;
        string contrasena = passwordInputField.text;
        StartCoroutine(RegisterUser(nombre, contrasena));
    }

    void Login()
    {
        string nombre = usernameInputField.text;
        string contrasena = passwordInputField.text;
        StartCoroutine(LoginUser(nombre, contrasena));
    }

    IEnumerator RegisterUser(string nombre, string contrasena)
    {
        string payload = "{\"nombre\":\"" + nombre + "\",\"contrasena\":\"" + contrasena + "\"}";

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/register", payload))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                ShowMessage(www.error);
            }
            else
            {
                ShowMessage("Usuario registrado exitosamente");
            }
        }
    }

    IEnumerator LoginUser(string nombre, string contrasena)
    {
        string payload = "{\"nombre\":\"" + nombre + "\",\"contrasena\":\"" + contrasena + "\"}";

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/login", payload))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                ShowMessage(www.error);
            }
            else
            {
                if (www.downloadHandler.text.Contains("Usuario autenticado"))
                {
                    // Extraer ID de usuario de la respuesta y guardarlo como entero
                    int userId = int.Parse(www.downloadHandler.text.Split(':')[1]);
                    PlayerPrefs.SetInt("userId", userId);
                    PlayerPrefs.Save();

                    ShowMessage("Usuario autenticado");
                    SceneManager.LoadScene("MenuInicial");
                }
                else
                {
                    ShowMessage("Usuario no autenticado");
                }
            }
        }
    }

    void ShowMessage(string message)
    {
        Estado.text = message;
        StartCoroutine(ShowMessageCoroutine());
    }

    IEnumerator ShowMessageCoroutine()
    {
        estadoCanvasGroup.alpha = 1;
        yield return new WaitForSeconds(1);
        estadoCanvasGroup.alpha = 0;
    }
}
