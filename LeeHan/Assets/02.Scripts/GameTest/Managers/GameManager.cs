using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
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
    public string record_scord;
    UIController UIManager => UIController.instance;
    WebSocketManager Web => WebSocketManager.instance;

    #region 서버와 관련된 것들
    
    public int loopCount;
    public MyData sendData = new MyData { message = "메세지 보내기" };
    [Serializable]
    public class MyData     //JSON 데이터를 위한 클래스
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!die && !string.IsNullOrEmpty(sendData.id))
        {
            time += Time.deltaTime;
        }
    }
    
    public void Player_die()
    {
        die = true;
        Time.timeScale = 0;
        //이건 테스트용
        Destination_Arrival();
    }
    public void Game_restart()
    {
        //여기에 플레이어 목숨, 생명력 처음으로 초기화하는 로직 추가해야됨 나머지 내용은 카톡에서 확인
      //  UIManager.restart_button.gameObject.SetActive(false);
        player.SetActive(true);
        time = 0;
        Time.timeScale = 1;
        die = false;
        player.transform.position = pos_init;
    }
    public void Destination_Arrival()
    {
        Debug.Log("기록");
        if (sendData.maximum_record < time)
        {
            Web.SendSocketMessage();
        }
    }
}
