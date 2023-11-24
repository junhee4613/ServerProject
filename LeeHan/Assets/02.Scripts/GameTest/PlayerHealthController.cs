using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    GameManager GameManager => GameManager.instance;
    public static PlayerHealthController instance;      //클래스의 모든 인스턴스가 동일한 값을 공유 

    private void Awake() //PlayerHealthController의 인스턴스에 쉽게 접근할 수 있도록 한다
    {
        instance = this;
    }

    public int currentHealth, maxHealth;    //플레이어 현재체력과 최대체력

    public float invincibilityLength = 1f;      //플레이어 피격 시 무적 시간
    private float invincibilityCounter;         //플레이어 피격 시 일정시간 무적

    public SpriteRenderer theSR;                //플레이어 스프라이트렌더러
    public Color normalColor, fadeColor;        //플레이어 피격됐을때 색상 변화

    private PlayerController thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GetComponent<PlayerController>();

        currentHealth = maxHealth;                          //최대체력을 현재체력에 할당

        UIController.instance.UpdateHealthDisplay(currentHealth, maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        if (invincibilityCounter > 0)       //플레이어 피격시 무적시간
        {
            invincibilityCounter -= Time.deltaTime;

            if (invincibilityCounter <= 0)      //플레이어 무적시간동안 색상변화
            {
                theSR.color = normalColor;
            }
        }
#if UNITY_EDITOR   //빌드시에는 포함안되게 처리 이거 지우면 빌드후에도 사용 가능
        if (Input.GetKeyDown(KeyCode.H))      //플레이어 힐
        {
            AddHealth(1);
        }
#endif      
    }


    public void DamagePlayer()   
    {
        if (invincibilityCounter <= 0)      //무적시간이 끝나면 다시 체력이 닳는다
        {

            currentHealth--;        //플레이어 체력깎기

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                GameManager.game_over();
                gameObject.SetActive(false);
                LifeController.instance.Respawn();      //플레이어 리스폰
            }
            else
            {
                invincibilityCounter = invincibilityLength;
                theSR.color = fadeColor;
                thePlayer.KnockBack();

                thePlayer.KnockBack();          //플레이어 넉백
            }

            UIController.instance.UpdateHealthDisplay(currentHealth, maxHealth);
        }

    }

    public void AddHealth(int amountToAdd)      
    {
        currentHealth += amountToAdd;       //플레이어 체력회복

        if (currentHealth > maxHealth)      //최대체력 이상 힐 안되게
        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdateHealthDisplay(currentHealth, maxHealth);

    }





}
