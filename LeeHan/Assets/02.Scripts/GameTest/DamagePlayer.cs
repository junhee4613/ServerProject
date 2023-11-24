using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private PlayerHealthController healthController;
    // Start is called before the first frame update
    void Start()
    {
        healthController = FindAnyObjectByType<PlayerHealthController>();  //PlayerHealthController 형식의 객체를 찾아서 할당
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)  //충돌 인식
    {
        if (other.CompareTag("Player"))                 //플레이어 태그를 가진 오브젝트와 충돌시 데미지를 준다
         {
            PlayerHealthController.instance.DamagePlayer();      //플레이어에게 데미지를 준다
        }




    }
}
