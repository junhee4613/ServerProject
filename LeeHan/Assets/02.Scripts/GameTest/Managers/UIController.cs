using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
  //  public Button restart_button;
    public Text time_record_text;
    public Text maximum_record_text;
    GameManager GameManager => GameManager.instance;
    private void Awake()
    {
        instance = this;
    }
    public Image[] heartIcons;

    public Sprite heartFull, heartEmpty;

    public TMP_Text livesText;

    public GameObject gameOverScreen;
    public TMP_Text ending_window_text;
    // Start is called before the first frame update
    void Start()
    {
        maximum_record_text.text = GameManager.max_data.maximum_name+ ":" + GameManager.max_data.maximum_num;
    }

    // Update is called once per frame
    void Update()
    {
        time_record_text.text = GameManager.time.ToString("F2") + "초";
    }

    public void UpdateHealthDisplay(int health , int maxHealth)
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].enabled = true;

            if (health > i)
            {
                heartIcons[i].sprite = heartFull;
            }
            else
            {
                heartIcons[i].sprite = heartEmpty;

                if (maxHealth <= i)
                {
                    heartIcons[i].enabled = false;
                }
            }

        }
    }
  
    public void UpdateLivesDisplay(int currentLives)       // 플레이어 목숨 텍스트로 표현 
    {
        livesText.text = currentLives.ToString();
    }

    public void ShowGameOver()
    {
        GameManager.die = true;
        gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        GameManager.time = 0;
        GameManager.die = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
