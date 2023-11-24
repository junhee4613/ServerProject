using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool die;
    public Action game_over;
    public Action restart;
    public float time;
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
    }
    public void Game_restart()
    {
        time = 0;
        Time.timeScale = 1;
        die = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
