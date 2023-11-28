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

    public float respawnDelay = 2f;     //������ ������ �ð�

    public int currentLives = 3;        //��� ����
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindFirstObjectByType<PlayerController>();

        if (UIController.instance != null)
        {
            UIController.instance.UpdateLivesDisplay(currentLives); //�÷��̾� ��� �ؽ�Ʈ

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

        thePlayer.gameObject.SetActive(false);      //�÷��̾� ������ �ڷ�ƾ ����
        thePlayer.theRB.velocity = Vector2.zero;    //�÷��̾� �װ� ��Ȱ �� ������ ���¿��� �ٽ� ����

        currentLives--; //��� �پ���

        if (currentLives > 0)       //����� �� �޸� �ڷ�ƾ ���� X
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
            UIController.instance.UpdateLivesDisplay(currentLives); //�÷��̾� ��� �ؽ�Ʈ

        }
    }

    public IEnumerator RespawnCo()
    {
        yield return new WaitForSeconds(respawnDelay);      //üũ����Ʈ ������ ������ 

        thePlayer.transform.position = FindFirstObjectByType<CheckPointManager>().respawnPosition; //�÷��̾ üũ����Ʈ�� ���ư��� �Ѵ�.

        PlayerHealthController.instance.AddHealth(PlayerHealthController.instance.maxHealth);  //üũ����Ʈ�� ���ư����� �ִ�ü������ ȸ��

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
