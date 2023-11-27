using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Text;
using Newtonsoft.Json;

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
        webSocket = new WebSocket("wss://port-0-leehan-node-20231014-jvpb2alnb1xslw.sel5.cloudtype.app");           //3000포트에 연결
        webSocket.OnOpen += OnWebSocketOpen;                        //함수 이벤트 등록
        webSocket.OnMessage += OnWebSocketMessage;                        //함수 이벤트 등록
        webSocket.OnClose += OnWebSocketClose;                        //함수 이벤트 등록

        webSocket.ConnectAsync();
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

        //JSON 데이터를 객체로 역직렬화
        GameManager.MyData receiveData = JsonConvert.DeserializeObject<GameManager.MyData>(jsonData);

        GameManager.InfoData infoData = JsonConvert.DeserializeObject<GameManager.InfoData>(jsonData);
        //테스트용@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        Debug.Log(receiveData.maximum_record);
        UIManager.maximum_record_text.text = receiveData.player_name + ":" + receiveData.maximum_record.ToString("F2");

        //여기가 최대 기록 할당 시키는 로직
        //UIManager.maximum_record_text.text = GameManager.sendData.player_name + ":" + GameManager.sendData.message;
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
    void OnWebSocketClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed");
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
        Debug.Log(GameManager.sendData.maximum_record + "최대기록");
        string jsonData = JsonConvert.SerializeObject(GameManager.sendData);
        webSocket.Send(jsonData);
    }
    
}
