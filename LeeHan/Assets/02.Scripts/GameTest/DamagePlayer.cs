using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private PlayerHealthController healthController;
    // Start is called before the first frame update
    void Start()
    {
        healthController = FindAnyObjectByType<PlayerHealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
         {
            //other.gameObject.SetActive(false);

            //FindFirstObjectByType<PlayerHealthController>().DamagePlayer();

            // healthController.DamagePlayer();

            PlayerHealthController.instance.DamagePlayer();
        }




    }
}
