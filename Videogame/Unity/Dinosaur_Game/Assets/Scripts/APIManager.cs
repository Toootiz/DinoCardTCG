
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class APIManager : MonoBehaviour
{
    private static APIManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public static APIManager instance {
        get {return _instance;}
    }
    
    public string apiResult; // Resultado de la llamada a la API
    [SerializeField] string url; // URL para conectar con el api
    [SerializeField] string getEndpoint; // Endpoint del api para sacar las cartas

    void Start()
    {
        // Solicitar los datos de la API al iniciar
        //GetData(url, getEndpoint);
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

    // Realizar una petición POST a la API
    public IEnumerator PostRequest(string url, string json, System.Action<string> callback)
    {
        Debug.Log(json);
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                callback(null);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }
    }
}


