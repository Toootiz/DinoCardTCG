using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI resultadoTexto;
    public TextMeshProUGUI turnosTexto;

    private bool won;

    void Start()
    {
        // Obtener el ID del jugador ganador de PlayerPrefs
        int winningPlayerId = PlayerPrefs.GetInt("LastWinningPlayerId", 0);
        // Obtener el ID del jugador desde PlayerPrefs
        int playerId = PlayerPrefs.GetInt("userId", 0);
        // Obtener la cantidad de turnos desde PlayerPrefs
        int turnCount = PlayerPrefs.GetInt("TurnCount", 0);

        // Actualizar el texto del resultado
        if (winningPlayerId == 4)
        {
            resultadoTexto.text = "Has perdido";
            won = false;
        }
        else
        {
            resultadoTexto.text = "Has ganado";
            won = true;
        }

        // Enviar la actualización de las estadísticas del jugador al servidor
        StartCoroutine(UpdatePlayerStats(playerId, won));
        // Enviar la cantidad de turnos al servidor
        StartCoroutine(SaveTurnCount(playerId, turnCount));
    }

    IEnumerator UpdatePlayerStats(int playerId, bool won)
    {
        string url = "http://localhost:3000/api/updatePlayerStats";
        var requestData = new PlayerStatsUpdate(playerId, won, 4);

        Debug.Log("Request Data: " + JsonUtility.ToJson(requestData));

        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log("JSON Data: " + jsonData); // Imprimir los datos JSON para verificar

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error updating player stats: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text); // Imprimir la respuesta del servidor
            }
            else
            {
                Debug.Log("Player stats updated successfully");
            }
        }
    }

    IEnumerator SaveTurnCount(int playerId, int turnCount)
    {
        string url = "http://localhost:3000/api/saveTurnCount";
        var requestData = new TurnCountData(playerId, turnCount);

        Debug.Log("Request Data: " + JsonUtility.ToJson(requestData));

        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log("JSON Data: " + jsonData); // Imprimir los datos JSON para verificar

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error saving turn count: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text); // Imprimir la respuesta del servidor
            }
            else
            {
                Debug.Log("Turn count saved successfully");
            }
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}

[System.Serializable]
public class PlayerStatsUpdate
{
    public int id_jugador;
    public bool gano;
    public int id_enemigo;

    public PlayerStatsUpdate(int id_jugador, bool gano, int id_enemigo)
    {
        this.id_jugador = id_jugador;
        this.gano = gano;
        this.id_enemigo = id_enemigo;
    }
}

[System.Serializable]
public class TurnCountData
{
    public int id_jugador;
    public int cantidad_turnos;

    public TurnCountData(int id_jugador, int cantidad_turnos)
    {
        this.id_jugador = id_jugador;
        this.cantidad_turnos = cantidad_turnos;
    }
}
