using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    private void Awake()
    {
        instance = this;
    }

    public int currentHealth, maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.UpdateHealthDisplay(currentHealth);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            gameObject.SetActive(false);
        }
        UIController.instance.UpdateHealthDisplay(currentHealth);
    }
}
