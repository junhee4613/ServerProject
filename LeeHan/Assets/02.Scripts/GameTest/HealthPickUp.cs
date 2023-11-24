using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healthToAdd;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth) //�÷��̾��� ���� ü���� �ִ�ü�°� ���� �������� ����
            {
                PlayerHealthController.instance.AddHealth(healthToAdd);     //ü��ȸ��

                Destroy(gameObject);
            }
      
        }
    }

}





