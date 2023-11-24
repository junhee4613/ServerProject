using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    GameManager GameManager => GameManager.instance;
    public static PlayerHealthController instance;      //Ŭ������ ��� �ν��Ͻ��� ������ ���� ���� 

    private void Awake() //PlayerHealthController�� �ν��Ͻ��� ���� ������ �� �ֵ��� �Ѵ�
    {
        instance = this;
    }

    public int currentHealth, maxHealth;    //�÷��̾� ����ü�°� �ִ�ü��

    public float invincibilityLength = 1f;      //�÷��̾� �ǰ� �� ���� �ð�
    private float invincibilityCounter;         //�÷��̾� �ǰ� �� �����ð� ����

    public SpriteRenderer theSR;                //�÷��̾� ��������Ʈ������
    public Color normalColor, fadeColor;        //�÷��̾� �ǰݵ����� ���� ��ȭ

    private PlayerController thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GetComponent<PlayerController>();

        currentHealth = maxHealth;                          //�ִ�ü���� ����ü�¿� �Ҵ�

        UIController.instance.UpdateHealthDisplay(currentHealth, maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        if (invincibilityCounter > 0)       //�÷��̾� �ǰݽ� �����ð�
        {
            invincibilityCounter -= Time.deltaTime;

            if (invincibilityCounter <= 0)      //�÷��̾� �����ð����� ����ȭ
            {
                theSR.color = normalColor;
            }
        }
#if UNITY_EDITOR   //����ÿ��� ���Ծȵǰ� ó�� �̰� ����� �����Ŀ��� ��� ����
        if (Input.GetKeyDown(KeyCode.H))      //�÷��̾� ��
        {
            AddHealth(1);
        }
#endif      
    }


    public void DamagePlayer()   
    {
        if (invincibilityCounter <= 0)      //�����ð��� ������ �ٽ� ü���� ��´�
        {

            currentHealth--;        //�÷��̾� ü�±��

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                GameManager.game_over();
                gameObject.SetActive(false);
                LifeController.instance.Respawn();      //�÷��̾� ������
            }
            else
            {
                invincibilityCounter = invincibilityLength;
                theSR.color = fadeColor;
                thePlayer.KnockBack();

                thePlayer.KnockBack();          //�÷��̾� �˹�
            }

            UIController.instance.UpdateHealthDisplay(currentHealth, maxHealth);
        }

    }

    public void AddHealth(int amountToAdd)      
    {
        currentHealth += amountToAdd;       //�÷��̾� ü��ȸ��

        if (currentHealth > maxHealth)      //�ִ�ü�� �̻� �� �ȵǰ�
        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdateHealthDisplay(currentHealth, maxHealth);

    }





}
