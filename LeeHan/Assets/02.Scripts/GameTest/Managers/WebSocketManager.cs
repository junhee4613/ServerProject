using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.Networking;
using System.Collections;

public class WebSocketManager : MonoBehaviour
{
    //remove.bg <= 뒷배경 제거해주는 사이트
    public WebSocket webSocket;
    bool IsConnected = false;
    int ConnectionAttempt = 0;
    const int MaxConnectionAttempts = 3;
    UIController UIManager => UIController.instance;

    GameManager GameManager => GameManager.instance;
    public static WebSocketManager instance;
    public const string apiUrl = "https://port-0-leehan-node-20231014-jvpb2alnb1xslw.sel5.cloudtype.app";
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
        
    }
    private void Start()
    {
        ConnectWebSock();
    }
    void ConnectWebSock()
    {
        try
        {
            webSocket = new WebSocket("ws://localhost:3000");
            webSocket.OnOpen += OnWebSocketOpen;
            webSocket.OnMessage += OnWebSocketMessage;
            webSocket.OnClose += OnWebSocketClose;

            webSocket.ConnectAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError("WebSocket connection error: " + ex.Message + "여기2");
        }
    }
    void OnWebSocketOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket connected");
        IsConnected = true;
        ConnectionAttempt = 0;
    }
    void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        string jsonData = Encoding.Default.GetString(e.RawData);
        Debug.Log("Received JSON data : " + jsonData);
        GameManager.MyData receiveData = JsonConvert.DeserializeObject<GameManager.MyData>(jsonData);
        GameManager.InfoData infoData = JsonConvert.DeserializeObject<GameManager.InfoData>(jsonData);
        GameManager.max_data.maximum_name = receiveData.player_name;
        GameManager.max_data.maximum_num = receiveData.maximum_record.ToString("F2");
        //StartCoroutine(Test());
        UIManager.maximum_record_text.text = receiveData.player_name + ":" + receiveData.maximum_record.ToString("F2");
        
        if (infoData != null)
        {
            string room = infoData.myParams.room;
            GameManager.loopCount = infoData.myParams.loopTimeCount;
        }

        if (receiveData != null && !string.IsNullOrEmpty(receiveData.clientID))
        {
            GameManager.sendData.clientID = receiveData.clientID;
        }
    }
    /*IEnumerator Test()
    {
        Debug.Log("DB에 저장");
        WWWForm form = new WWWForm();
        form.AddField("player_name", GameManager.max_data.maximum_name);
        form.AddField("maximum_record", GameManager.max_data.maximum_num);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl + "/maximum_record", form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(webRequest.downloadHandler.text);
                UIManager.maximum_record_text.text = res.message;
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                GameManager.Maxmum_data res2 = JsonConvert.DeserializeObject<GameManager.Maxmum_data>(responseText);
                GameManager.max_data = res2;
                Debug.Log(GameManager.max_data + "DB에 저장");
            }
        }
    }*/
    void OnWebSocketClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed with code: " + e.Code + ", reason: " + e.Reason + "여기");
        IsConnected = false;

        if (ConnectionAttempt < MaxConnectionAttempts)
        {
            ConnectionAttempt++;
            Debug.Log("Attemping to reconnect . Attemp" + ConnectionAttempt);
            ConnectWebSock();
        }
        else
        {
            Debug.Log("Failed to connect ager " + MaxConnectionAttempts + "attempts. ");
        }
    }
    void OnAplicationQuit()             //어플리케이션 종료되면 소캣 제거
    {
        DisconnectWebSocket();
    }
    void DisconnectWebSocket()
    {
        if (webSocket != null && IsConnected)
        {
            webSocket.Close();
            IsConnected = false;
        }
    }
    public void SendSocketMessage()
    {
        GameManager.sendData.requestType = 0;
        GameManager.sendData.maximum_record = GameManager.time;
        string jsonData = JsonConvert.SerializeObject(GameManager.sendData);
        webSocket.Send(jsonData);

    }
    
}
