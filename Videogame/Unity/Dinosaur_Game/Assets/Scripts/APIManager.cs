using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class APIManager : MonoBehaviour
{
    public string apiResult; // Resultado de la llamada a la API
    [SerializeField] string url; // URL para conectar con el api
    [SerializeField] string getEndpoint; // Endpoint del api para sacar las cartas

    void Start()
    {
        // Solicitar los datos de la API al iniciar
        GetData(url, getEndpoint);
    }

    // Obtener datos de la API
    public void GetData(string url, string getEndpoint)
    {
        StartCoroutine(RequestGet(url + getEndpoint));
    }

    // Realizar una petición GET a la API
    IEnumerator RequestGet(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Request failed: " + www.error);
            }
            else
            {
                apiResult = www.downloadHandler.text;
                Debug.Log("The response was: " + apiResult);
                // Guardar los datos en PlayerPrefs
                PlayerPrefs.SetString("CardData", apiResult);
                PlayerPrefs.Save();
            }
        }
    }
}
