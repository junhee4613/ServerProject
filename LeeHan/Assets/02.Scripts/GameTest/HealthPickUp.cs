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
            if (PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth) //플레이어의 현재 체력이 최대체력과 같지 않을떄만 동작
            {
                PlayerHealthController.instance.AddHealth(healthToAdd);     //체력회복

                Destroy(gameObject);
            }
      
        }
    }

}






