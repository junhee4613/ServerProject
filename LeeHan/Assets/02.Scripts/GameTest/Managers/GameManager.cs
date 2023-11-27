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
    UIController UIManager => UIController.instance;

    #region 서버와 관련된 것들
    WebSocket webSocket;
    bool IsConnected = false;
    int ConnectionAttempt = 0;
    const int MaxConnectionAttempts = 3;
    public int loopCount;
    public MyData sendData = new MyData { message = "메세지 보내기" };
    public float maximum_record = 0;
    [Serializable]
    public class MyData     //JSON 데이터를 위한 클래스
    {
        public string player_name;
        public string id;
        public float my_maximum_record = 0;
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
        webSocket = new WebSocket("ws://localhost:3000");           //3000포트에 연결
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
        MyData receiveData = JsonConvert.DeserializeObject<MyData>(jsonData);

        InfoData infoData = JsonConvert.DeserializeObject<InfoData>(jsonData);
        //여기가 최대 기록 할당 시키는 로직
        UIManager.maximum_record_text.text = sendData.player_name + ":" + sendData.message;
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
        sendData.requestType = 0;
        sendData.message = time.ToString("F2");
        string jsonData = JsonConvert.SerializeObject(sendData);

        webSocket.Send(jsonData);
    }
    #endregion


    public GameObject player;
    public Vector2 pos_init;
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
        if (!die && !string.IsNullOrEmpty(sendData.id))
        {
            time += Time.deltaTime;
        }
    }
    public void Destination_Arrival()
    {
        if (webSocket == null || !IsConnected)
        {
            return;
        }
        if (maximum_record < time)
        {
            SendSocketMessage();
        }
    }
    public void Player_die()
    {
        die = true;
        Time.timeScale = 0;
    }
    public void Game_restart()
    {
        //여기에 플레이어 목숨, 생명력 처음으로 초기화하는 로직 추가해야됨 나머지 내용은 카톡에서 확인
        UIManager.restart_button.gameObject.SetActive(false);
        player.SetActive(true);
        time = 0;
        Time.timeScale = 1;
        die = false;
        player.transform.position = pos_init;
    }
}
