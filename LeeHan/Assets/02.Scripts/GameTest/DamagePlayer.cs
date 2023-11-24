using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private PlayerHealthController healthController;
    // Start is called before the first frame update
    void Start()
    {
        healthController = FindAnyObjectByType<PlayerHealthController>();  //PlayerHealthController ������ ��ü�� ã�Ƽ� �Ҵ�
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)  //�浹 �ν�
    {
        if (other.CompareTag("Player"))                 //�÷��̾� �±׸� ���� ������Ʈ�� �浹�� �������� �ش�
         {
            PlayerHealthController.instance.DamagePlayer();      //�÷��̾�� �������� �ش�
        }




    }
}
