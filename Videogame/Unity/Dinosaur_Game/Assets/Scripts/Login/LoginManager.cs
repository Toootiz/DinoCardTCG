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

    void Start()
    {
        registerButton.onClick.AddListener(Register);
        loginButton.onClick.AddListener(Login);
    }


    void Register()
    {
        string nombre = usernameInputField.text;
        string contrasena = passwordInputField.text;

        StartCoroutine(RegisterUser(nombre, contrasena));
    }
    
/*
    //public bool l;
    public void SignIn(){
        //string nombre;
        //string password;

        Debug.Log(APIManager.instance.karen);
        APIManager.instance.PostRequest("http://localhost:3000/login", "{nombre:nombre, password:password}", null);
    }
    */

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
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Usuario registrado exitosamente");
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
                Debug.Log(www.error);
            }
            else
            {
                if (www.downloadHandler.text == "Usuario autenticado")
                {
                    Debug.Log("Usuario autenticado");
                    SceneManager.LoadScene("MenuInicial");
                }
                else
                {
                    Debug.Log("Usuario no autenticado");
                }
            }
        }
    }
}
