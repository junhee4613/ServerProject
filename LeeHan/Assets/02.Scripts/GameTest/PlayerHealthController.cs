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

    public float invincibilityLength = 1f;
    private float invincibilityCounter;

    public SpriteRenderer theSR;
    public Color normalColor, fadeColor;

    private PlayerController thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GetComponent<PlayerController>();

        currentHealth = maxHealth;

        UIController.instance.UpdateHealthDisplay(currentHealth, maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            if (invincibilityCounter <= 0)
            {
                theSR.color = normalColor;
            }
        }
    }

    public void DamagePlayer()
    {
        if (invincibilityCounter <= 0)
        {

            currentHealth--;

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                gameObject.SetActive(false);
            }
            else
            {
                invincibilityCounter = invincibilityLength;

                theSR.color = fadeColor;

                thePlayer.KnockBack();
            }

            UIController.instance.UpdateHealthDisplay(currentHealth, maxHealth);
        }

    }
}
