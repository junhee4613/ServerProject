using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Button restart_button;
    public Text time_record;
    GameManager GameManager => GameManager.instance;
    private void Awake()
    {
        instance = this;
    }
    public Image[] heartIcons;

    public Sprite heartFull, heartEmpty;

    public TMP_Text livesText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.game_over += Restart_Button;
        restart_button.onClick.AddListener(() => GameManager.restart());
    }

    // Update is called once per frame
    void Update()
    {
       
        time_record.text = GameManager.time.ToString("F2") + "초";
        if(restart_button == null)
        {
            Debug.Log("널이다");

        }
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
    public void Restart_Button()
    {
        if(restart_button != null)
        {
            restart_button.gameObject.SetActive(true);
        }
    }
    public void UpdateLivesDisplay(int currentLives)       // 플레이어 목숨 텍스트로 표현 
    {
        livesText.text = currentLives.ToString();
    }

}
