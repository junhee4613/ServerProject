using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public static LifeController instance;
    GameManager GameManager => GameManager.instance;
    private void Awake()
    {
        instance = this;
    }

    private PlayerController thePlayer;

    public float respawnDelay = 2f;     //리스폰 딜레이 시간

    public int currentLives = 3;        //목숨 개수
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindFirstObjectByType<PlayerController>();

        if (UIController.instance != null)
        {
            UIController.instance.UpdateLivesDisplay(currentLives); //플레이어 목숨 텍스트

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        // thePlayer.transform.position = FindAnyObjectByType<CheckPointManager>().respawnPosition;  

        //PlayerHealthController.instance.AddHealth(PlayerHealthController.instance.maxHealth);

        thePlayer.gameObject.SetActive(false);      //플레이어 죽음시 코루틴 실행
        thePlayer.theRB.velocity = Vector2.zero;    //플레이어 죽고 부활 시 정지된 상태에서 다시 시작

        currentLives--; //목숨 줄어들기

        if (currentLives > 0)       //목숨이 다 달면 코루틴 실행 X
        {
            StartCoroutine(RespawnCo());

        }
        else
        {
            currentLives = 0;
            StartCoroutine(GameOverCo());
        }
        if (UIController.instance != null)
        {
            UIController.instance.UpdateLivesDisplay(currentLives); //플레이어 목숨 텍스트

        }
    }

    public IEnumerator RespawnCo()
    {
        yield return new WaitForSeconds(respawnDelay);      //체크포인트 리스폰 딜레이 

        thePlayer.transform.position = FindFirstObjectByType<CheckPointManager>().respawnPosition; //플레이어를 체크포인트로 돌아가게 한다.

        PlayerHealthController.instance.AddHealth(PlayerHealthController.instance.maxHealth);  //체크포인트로 돌아갔을때 최대체력으로 회복

        thePlayer.gameObject.SetActive(true);
    }

    public IEnumerator GameOverCo()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (UIController.instance != null)
        {
            UIController.instance.ShowGameOver();
        }
    }

}
