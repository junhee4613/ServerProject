using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool die;
    public Action game_over;
    public Action restart;
    public float time;


    #region ������ ���õ� �͵�
    WebSocket webSocket;
    bool IsConnected = false;
    int ConnectionAttempt = 0;
    const int MaxConnectionAttempts = 3;
    public int loopCount;
    public MyData sendData = new MyData { message = "�޼��� ������" };
    [Serializable]
    public class MyData     //JSON �����͸� ���� Ŭ����
    {
        public string player_name;
        public string id;
        public float maximum_record = 0;
        public string clientID;
        public string message;
        public string account_name;
        public int requestType;
    }
    public class InfoData
    {
        public string type;
        public InfoParams myParams;
    }
    public class InfoParams
    {
        public string room;
        public int loopTimeCount;
    }
    void ConnectWebSock()
    {
        webSocket = new WebSocket("ws://localhost:3000");           //3000��Ʈ�� ����
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
        MyData receiveData = JsonConvert.DeserializeObject<MyData>(jsonData);

        InfoData infoData = JsonConvert.DeserializeObject<InfoData>(jsonData);

        if (infoData != null)
        {
            string room = infoData.myParams.room;
            loopCount = infoData.myParams.loopTimeCount;
        }

        if (receiveData != null && !string.IsNullOrEmpty(receiveData.clientID))
        {
            sendData.clientID = receiveData.clientID;
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
        sendData.requestType = 0;
        sendData.message = time.ToString("F2");
        string jsonData = JsonConvert.SerializeObject(sendData);

        webSocket.Send(jsonData);
    }
    #endregion


    GameObject player;
    Vector2 pos_init;
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
        game_over += Player_die;
        restart += Game_restart;

    }
    // Start is called before the first frame update
    void Start()
    {
        if (player = GameObject.FindGameObjectWithTag("Player"))
        {
            pos_init = player.transform.position;
        }
        ConnectWebSock();
    }

    // Update is called once per frame
    void Update()
    {
        if (!die)
        {
            time += Time.deltaTime;
        }
    }
    public void Player_die()
    {
        die = true;
        Time.timeScale = 0;
        if (webSocket == null || !IsConnected)
        {
            return;
        }
        if (sendData.maximum_record < time)
        {
            SendSocketMessage();
        }
    }
    public void Game_restart()
    {
        //���⿡ �÷��̾� ��� �� ������ ��� ó������ �ʱ�ȭ�ϴ� ���� �߰��ؾߵ�
        time = 0;
        Time.timeScale = 1;
        die = false;
        player.transform.position = pos_init;
    }
}
