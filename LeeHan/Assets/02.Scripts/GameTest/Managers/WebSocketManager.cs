using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Text;
using Newtonsoft.Json;

public class WebSocketManager : MonoBehaviour
{
    //remove.bg <= �޹�� �������ִ� ����Ʈ
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
        webSocket = new WebSocket("wss://port-0-leehan-node-20231014-jvpb2alnb1xslw.sel5.cloudtype.app");           //3000��Ʈ�� ����
        webSocket.OnOpen += OnWebSocketOpen;                        //�Լ� �̺�Ʈ ���
        webSocket.OnMessage += OnWebSocketMessage;                        //�Լ� �̺�Ʈ ���
        webSocket.OnClose += OnWebSocketClose;                        //�Լ� �̺�Ʈ ���

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

        //JSON �����͸� ��ü�� ������ȭ
        GameManager.MyData receiveData = JsonConvert.DeserializeObject<GameManager.MyData>(jsonData);

        GameManager.InfoData infoData = JsonConvert.DeserializeObject<GameManager.InfoData>(jsonData);
        //�׽�Ʈ��@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        Debug.Log(receiveData.maximum_record);
        UIManager.maximum_record_text.text = receiveData.player_name + ":" + receiveData.maximum_record.ToString("F2");

        //���Ⱑ �ִ� ��� �Ҵ� ��Ű�� ����
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
    void OnAplicationQuit()             //���ø����̼� ����Ǹ� ��Ĺ ����
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
        Debug.Log(GameManager.sendData.maximum_record + "�ִ���");
        string jsonData = JsonConvert.SerializeObject(GameManager.sendData);
        webSocket.Send(jsonData);
    }
    
}
