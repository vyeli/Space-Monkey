using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    public int currentHealth, maxHealth;

    public float invincibleLength;
    private float invincibleCounter;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        /*
        if (UiManager.instance.healthText.text != currentHealth.ToString())
        {
            UiManager.instance.healthText.text = HealthManager.instance.currentHealth.ToString();
        }
        */
        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;


            for(int i = 0; i < Player.instance.playerPieces.Length; i++)
            {
                if (Mathf.Floor(invincibleCounter * 5f) % 2 == 0)
                {
                    Player.instance.playerPieces[i].SetActive(true);
                } else
                {
                    Player.instance.playerPieces[i].SetActive(false);
                }

                if (invincibleCounter <= 0)
                {
                    Player.instance.playerPieces[i].SetActive(true);
                }
            }


        }
    }

    public void HurtPlayer()
    {
        if (invincibleCounter <= 0)
        {
            currentHealth--;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                GameManager.instance.RespawnPlayer();
            } else
            {
                // TODO: Knockback
                // Player.instance.KnockBack();
                invincibleCounter = invincibleLength;
            }
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void AddHealth(int amountToHeal)
    {
        currentHealth += amountToHeal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}
